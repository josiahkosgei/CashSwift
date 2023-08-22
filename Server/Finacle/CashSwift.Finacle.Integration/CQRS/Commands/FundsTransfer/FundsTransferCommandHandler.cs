using AutoMapper;
using BSAccountFundsTransferServiceReference;
using CashSwift.Finacle.Integration.CQRS.DTOs.FundsTransfer;
using CashSwift.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using CashSwift.Finacle.Integration.CQRS.Events;
using CashSwift.Finacle.Integration.CQRS.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using System.ServiceModel;
using FundsTransferRequestHeaderType = BSAccountFundsTransferServiceReference.RequestHeaderType;

namespace CashSwift.Finacle.Integration.CQRS.Commands.FundsTransfer
{
    public class FundsTransferCommandHandler : IRequestHandler<FundsTransferCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FundsTransferCommandHandler> _logger;
        private readonly SOAServerConfiguration _soaServerConfiguration;
        private readonly IMapper _mapper;
        public FundsTransferCommandHandler(IMapper mapper, IMediator mediator, ILogger<FundsTransferCommandHandler> logger, IOptionsMonitor<SOAServerConfiguration> optionsMonitor)
        {
            _mediator = mediator;
            _logger = logger;
            _soaServerConfiguration = optionsMonitor.CurrentValue;
            _mapper = mapper;
        }
        public async Task<int> Handle(FundsTransferCommand request, CancellationToken cancellationToken)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            var remoteAddress = new EndpointAddress(_soaServerConfiguration.PostConfiguration.ServerURI);
            //validation
            var bsAccountClient = new BSAccountClient(binding, remoteAddress);
            var requestHeaderTypeDto = new RequestHeaderTypeDto()
            {

                CorrelationID = Guid.NewGuid().ToString(),
                CreationTimestamp = DateTime.Now.ToUniversalTime(),
                MessageID = Guid.NewGuid().ToString(),
                Credentials = new CredentialsTypeDto
                {
                    BankID = "01",
                    SystemCode = _soaServerConfiguration.AccountValidationConfiguration.SystemCode,
                    Password = _soaServerConfiguration.AccountValidationConfiguration.Password,
                    Username = _soaServerConfiguration.AccountValidationConfiguration.Username,
                },

            };

            var fundsTransfer = _mapper.Map<FundsTransferType>(request.FundsTransfer);
            var requestHeaderType = _mapper.Map<FundsTransferRequestHeaderType>(requestHeaderTypeDto);
            var postRequest = new PostRequest
            {
                FundsTransfer = fundsTransfer,
                RequestHeader = requestHeaderType
            };
            var response = await bsAccountClient.PostAsync(postRequest);

            //mapper
            var validateAccountResponseDto = _mapper.Map<FundsTransferResponseDto>(response);

            //notification
            await _mediator.Publish(new FundsTransfered(validateAccountResponseDto));

            return 1;
        }
    }
}
