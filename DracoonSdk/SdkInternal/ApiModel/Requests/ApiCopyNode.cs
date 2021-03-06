using Newtonsoft.Json;

namespace Dracoon.Sdk.SdkInternal.ApiModel.Requests {
    internal class ApiCopyNode {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long NodeId {
            get; set;
        }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string NewName {
            get; set;
        }
    }
}
