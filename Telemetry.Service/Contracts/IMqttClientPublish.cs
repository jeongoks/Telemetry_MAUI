using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telemetry.Service.Contracts
{
    public interface IMqttClientPublish
    {
        Task PublishMessage(string message);
    }
}
