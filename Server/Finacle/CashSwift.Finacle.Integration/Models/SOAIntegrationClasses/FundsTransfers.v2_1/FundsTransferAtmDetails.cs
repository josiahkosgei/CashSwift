// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAtmDetails
using System.ComponentModel;
using System.Xml.Serialization;


namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAtmDetails
    {
        private FundsTransferAtmDetailsProcessingCode processingCodeField;

        private FundsTransferAtmDetailsAmountTrans amountTransField;

        private FundsTransferAtmDetailsAmountRecon amountReconField;

        private string reconRateField;

        private FundsTransferAtmDetailsAmountCardHldBill amountCardHldBillField;

        private DateTime trmDateTimeField;

        private DateTime txnDateTimeField;

        private DateTime cardEffDateField;

        private DateTime cardExpDateField;

        private DateTime captureDateField;

        private FundsTransferAtmDetailsPtofSerDataCd ptofSerDataCdField;

        private string msgReasonField;

        private string cardSeqNoField;

        private string functionCodeField;

        private DateTime dateReconField;

        private FundsTransferAtmDetailsOriginalTxnAmt originalTxnAmtField;

        private string accIDCField;

        private string track2DataField;

        private string retRefNumField;

        private string apprCodeField;

        private string atmActionCdField;

        private string cardAccpTerIdField;

        private string cardAccpIdCdField;

        private string cardAccpNameField;

        private string adddResCdField;

        private string track1DataField;

        private FundsTransferAtmDetailsFeeAmt feeAmtField;

        private string pINDataField;

        private FundsTransferAtmDetailsOriginalDataEle originalDataEleField;

        private string aAgentIDCField;

        private string transportDataField;

        private string terminalDataField;

        private string cardIssAuthDataField;

        private string recvIICField;

        private string accIdentification1Field;

        private string accIdentification2Field;

        private string txnSpecDataField;

        public FundsTransferAtmDetailsProcessingCode processingCode
        {
            get
            {
                return processingCodeField;
            }
            set
            {
                processingCodeField = value;
            }
        }

        public FundsTransferAtmDetailsAmountTrans amountTrans
        {
            get
            {
                return amountTransField;
            }
            set
            {
                amountTransField = value;
            }
        }

        public FundsTransferAtmDetailsAmountRecon amountRecon
        {
            get
            {
                return amountReconField;
            }
            set
            {
                amountReconField = value;
            }
        }

        public string reconRate
        {
            get
            {
                return reconRateField;
            }
            set
            {
                reconRateField = value;
            }
        }

        public FundsTransferAtmDetailsAmountCardHldBill amountCardHldBill
        {
            get
            {
                return amountCardHldBillField;
            }
            set
            {
                amountCardHldBillField = value;
            }
        }

        public DateTime trmDateTime
        {
            get
            {
                return trmDateTimeField;
            }
            set
            {
                trmDateTimeField = value;
            }
        }

        public DateTime txnDateTime
        {
            get
            {
                return txnDateTimeField;
            }
            set
            {
                txnDateTimeField = value;
            }
        }

        public DateTime cardEffDate
        {
            get
            {
                return cardEffDateField;
            }
            set
            {
                cardEffDateField = value;
            }
        }

        public DateTime cardExpDate
        {
            get
            {
                return cardExpDateField;
            }
            set
            {
                cardExpDateField = value;
            }
        }

        public DateTime captureDate
        {
            get
            {
                return captureDateField;
            }
            set
            {
                captureDateField = value;
            }
        }

        public FundsTransferAtmDetailsPtofSerDataCd ptofSerDataCd
        {
            get
            {
                return ptofSerDataCdField;
            }
            set
            {
                ptofSerDataCdField = value;
            }
        }

        public string msgReason
        {
            get
            {
                return msgReasonField;
            }
            set
            {
                msgReasonField = value;
            }
        }

        public string cardSeqNo
        {
            get
            {
                return cardSeqNoField;
            }
            set
            {
                cardSeqNoField = value;
            }
        }

        public string functionCode
        {
            get
            {
                return functionCodeField;
            }
            set
            {
                functionCodeField = value;
            }
        }

        public DateTime dateRecon
        {
            get
            {
                return dateReconField;
            }
            set
            {
                dateReconField = value;
            }
        }

        public FundsTransferAtmDetailsOriginalTxnAmt originalTxnAmt
        {
            get
            {
                return originalTxnAmtField;
            }
            set
            {
                originalTxnAmtField = value;
            }
        }

        public string accIDC
        {
            get
            {
                return accIDCField;
            }
            set
            {
                accIDCField = value;
            }
        }

        public string track2Data
        {
            get
            {
                return track2DataField;
            }
            set
            {
                track2DataField = value;
            }
        }

        public string retRefNum
        {
            get
            {
                return retRefNumField;
            }
            set
            {
                retRefNumField = value;
            }
        }

        public string apprCode
        {
            get
            {
                return apprCodeField;
            }
            set
            {
                apprCodeField = value;
            }
        }

        public string atmActionCd
        {
            get
            {
                return atmActionCdField;
            }
            set
            {
                atmActionCdField = value;
            }
        }

        public string cardAccpTerId
        {
            get
            {
                return cardAccpTerIdField;
            }
            set
            {
                cardAccpTerIdField = value;
            }
        }

        public string cardAccpIdCd
        {
            get
            {
                return cardAccpIdCdField;
            }
            set
            {
                cardAccpIdCdField = value;
            }
        }

        public string cardAccpName
        {
            get
            {
                return cardAccpNameField;
            }
            set
            {
                cardAccpNameField = value;
            }
        }

        public string adddResCd
        {
            get
            {
                return adddResCdField;
            }
            set
            {
                adddResCdField = value;
            }
        }

        public string track1Data
        {
            get
            {
                return track1DataField;
            }
            set
            {
                track1DataField = value;
            }
        }

        public FundsTransferAtmDetailsFeeAmt feeAmt
        {
            get
            {
                return feeAmtField;
            }
            set
            {
                feeAmtField = value;
            }
        }

        public string pINData
        {
            get
            {
                return pINDataField;
            }
            set
            {
                pINDataField = value;
            }
        }

        public FundsTransferAtmDetailsOriginalDataEle originalDataEle
        {
            get
            {
                return originalDataEleField;
            }
            set
            {
                originalDataEleField = value;
            }
        }

        public string aAgentIDC
        {
            get
            {
                return aAgentIDCField;
            }
            set
            {
                aAgentIDCField = value;
            }
        }

        public string transportData
        {
            get
            {
                return transportDataField;
            }
            set
            {
                transportDataField = value;
            }
        }

        public string terminalData
        {
            get
            {
                return terminalDataField;
            }
            set
            {
                terminalDataField = value;
            }
        }

        public string cardIssAuthData
        {
            get
            {
                return cardIssAuthDataField;
            }
            set
            {
                cardIssAuthDataField = value;
            }
        }

        public string recvIIC
        {
            get
            {
                return recvIICField;
            }
            set
            {
                recvIICField = value;
            }
        }

        public string accIdentification1
        {
            get
            {
                return accIdentification1Field;
            }
            set
            {
                accIdentification1Field = value;
            }
        }

        public string accIdentification2
        {
            get
            {
                return accIdentification2Field;
            }
            set
            {
                accIdentification2Field = value;
            }
        }

        public string txnSpecData
        {
            get
            {
                return txnSpecDataField;
            }
            set
            {
                txnSpecDataField = value;
            }
        }
    }
}
