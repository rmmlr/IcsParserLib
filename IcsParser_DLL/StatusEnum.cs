namespace Rca.IcsParser
{
	/// <summary>
	/// Enum für den Event-Status
	/// </summary>
	public enum StatusEnum
	{
        /// <summary>
        /// Status im Eventeintag nicht gesetzt bzw. nicht RFC2445-Konform
        /// </summary>
		Default = 0,
        /// <summary>
        /// Bestätigt
        /// </summary>
		Confirmed,
        /// <summary>
        /// Abgesagt
        /// </summary>
		Cancelled,
        /// <summary>
        /// Vorläufig
        /// </summary>
		Tentative
	}
}
