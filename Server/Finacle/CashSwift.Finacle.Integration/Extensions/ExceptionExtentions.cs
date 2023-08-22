namespace CashSwift.Finacle.Integration.Extensions
{
    public static class ExceptionExtentions
	{
		public static string MessageString(this Exception ex, int? MaxCharacters = null)
		{
			if (MaxCharacters.HasValue && MaxCharacters > 0)
			{
				return (ex.Message + ">" + ex.InnerException?.Message + ">" + ex.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.InnerException?.Message).Left(MaxCharacters.Value);
			}
			return ex.Message + ">" + ex.InnerException?.Message + ">" + ex.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.Message + ">" + ex.InnerException?.InnerException?.InnerException?.InnerException?.Message;
		}
	}

}
