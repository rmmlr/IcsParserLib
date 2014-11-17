namespace Rca.IcsParser
{
	/// <summary>
	/// Enum für den Event-Status
	/// </summary>
	public enum StatusEnum
	{
		Default = 0,    //Status im Eventeintag nicht gesetzt bzw. nicht RFC2445-Konform
		Confirmed,
		Cancelled,
		Tentative
	}
}
