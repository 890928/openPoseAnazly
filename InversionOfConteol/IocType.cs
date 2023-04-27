namespace Webapi.InversionOfControl
{
    public enum IocType
    {
        /// <summary>
        /// The instance is alive in all web process
        /// </summary>
        Singleton,

        /// <summary>
        /// The instance is alive in this http request.
        /// </summary>
        Scoped,

        /// <summary>
        /// Everytime you call the instance is a new instance.
        /// </summary>
        Transient,
    }
}