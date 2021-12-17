// *******************************************************************************
// <copyright file="Client.cs" company="Intuit">
// Copyright (c) 2021 Intuit
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
// *******************************************************************************
using Newtonsoft.Json;

namespace ClientsApiExample.Model
{
    [JsonObject]
    public class Client
    {
        [JsonIgnore]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("phone")]
        public Phone Phone { get; set; }

        [JsonProperty("mobile")]
        public Phone Mobile { get; set; }

        [JsonProperty("email")]
        public Email Email { get; set; }

        [JsonProperty("shippingAddress")]
        public ShippingAddress ShippingAddress { get; set; }

        [JsonProperty("billingAddress")]
        public BillingAddress BillingAddress { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("organizationName")]
        public string OrganizationName { get; set; }

        [JsonProperty("__typename")]
        private string _TypeName
        {
            // get intentionally ommitted
            // we only want to deserialize this value, and not serialize it
            set { Type = value; }
        }
    }
}
