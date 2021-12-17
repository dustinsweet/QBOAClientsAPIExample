// *******************************************************************************
// <copyright file="ShippingAddress.cs" company="Intuit">
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
using System.Collections.Generic;

namespace ClientsApiExample.Model
{
    [JsonObject]
    public class ShippingAddress
    {
        [JsonProperty("primary")]
        public bool Primary { get; set; }

        [JsonProperty("freeFormAddressLine")]
        public string FreeFormAddressLine { get; set; }

        [JsonProperty("contactMethodTypes")]
        public List<string> ContactMethodTypes { get; set; }
    }
}
