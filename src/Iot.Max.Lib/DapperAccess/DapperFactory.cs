using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iot.Max.Lib
{
    public class DapperFactory:IDapperFactory
    {
        private readonly IServiceProvider _services;
        private readonly IOptionsMonitor<DapperFactoryOptions> _optionsMonitor;

        public DapperFactory(IServiceProvider services, IOptionsMonitor<DapperFactoryOptions> optionsMonitor)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        }

        //public DapperClientHelper CreateClient(string name)
        //{
        public DapperClientHelper CreateClient()
        {
            var client = new DapperClientHelper(new ConnectionConfig { });
            var name = client.CurrentConnectionConfig.DbType.ToString().ToLower();
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var option = _optionsMonitor.Get(name).DapperActions.FirstOrDefault();     
            //var option = _optionsMonitor.CurrentValue.DapperActions.FirstOrDefault();
            if (option != null)
                option(client.CurrentConnectionConfig);
            else
                throw new ArgumentNullException(nameof(option));

            return client;
        }
    }
}
