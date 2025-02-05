using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace OpenMessage.Builders
{
    /// <summary>
    ///     Defines a common base for a consumer builder.
    /// </summary>
    public abstract class Builder : IBuilder
    {
        /// <summary>
        ///     The unique ID associated with this consumer.
        /// </summary>
        public string ConsumerId { get; } = Guid.NewGuid()
                                                .ToString("N");

        /// <summary>
        ///     The underlying host builder.
        /// </summary>
        public IMessagingBuilder HostBuilder { get; }

        /// <summary>
        ///     ctor
        /// </summary>
        public Builder(IMessagingBuilder hostBuilder) => HostBuilder = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));

        /// <summary>
        ///     Build the consumer
        /// </summary>
        public abstract void Build();

        /// <summary>
        ///     Use the specified action to configure options for this consumer.
        /// </summary>
        /// <param name="configurator">The options configuration action</param>
        /// <param name="defaultOptions">Determines whether or not to setup the default options. Default: false</param>
        /// <typeparam name="T">The type of options to configure</typeparam>
        protected void ConfigureOptions<T>(Action<HostBuilderContext, T> configurator, bool defaultOptions = false)
            where T : class
        {
            if (configurator is null)
                return;

            if (!defaultOptions)
                HostBuilder.Services.Configure<T>(ConsumerId, options => configurator(HostBuilder.Context, options));
            else
                HostBuilder.Services.Configure<T>(options => configurator(HostBuilder.Context, options));
        }
    }
}