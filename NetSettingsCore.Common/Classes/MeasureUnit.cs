namespace NetSettings.Common.Classes
{
    public enum MeasureUnit
    {
        /// <summary>
        /// Specifies the world unit as the unit of measure.
        /// </summary>
        World = 0,
        /// <summary>
        /// Specifies 1/75 inch as the unit of measure.
        /// </summary>
        Display = 1,
        /// <summary>
        /// Specifies a device pixel as the unit of measure.
        /// </summary>
        Pixel = 2,
        /// <summary>
        /// Specifies a printer's point (1/72 inch) as the unit of measure.
        /// </summary>
        Point = 3,
        /// <summary>
        /// Specifies the inch as the unit of measure.
        /// </summary>
        Inch = 4,
        /// <summary>
        /// Specifies the document unit (1/300 inch) as the unit of measure.
        /// </summary>
        Document = 5,
        /// <summary>
        /// Specifies the millimeter as the unit of measure.
        /// </summary>
        Millimeter = 6
    }
}