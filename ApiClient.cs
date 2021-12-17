// *******************************************************************************
// <copyright file="ApiClient.cs" company="Intuit">
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
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using ClientsApiExample.Model;
using Newtonsoft.Json;
using GraphQL;
using System.Linq;

namespace Intuit.ClientsApiExample
{
    internal class ApiClient
    {
        private const string API_URL = "https://public.api.intuit.com/2020-04/graphql";
        private const string BEARER_TOKEN = "<SET_VALUE_HERE>";

        private GraphQLHttpClient _graphQlClient;

        internal ApiClient()
        {
            _graphQlClient = new GraphQLHttpClient(API_URL, new NewtonsoftJsonSerializer());
            _graphQlClient.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", BEARER_TOKEN);
        }


        internal async Task<List<Client>> GetClientsAsync(string filter = "")
        {
            string query = @"
                {
                  firm {
                    firmName" +
                  $"\nclients{filter} {{\n" +
                      @"nodes {
                            __typename
                            id
                            active
                            phone {
                                    number
                                    type
                                    primary
                                    contactMethodTypes
                            }
                            mobile {
                                    number
                                    type
                                    primary
                                    contactMethodTypes
                            }
                            email {
                                    address
                                    contactMethodTypes
                            }
 
                            shippingAddress {
                                    primary
                                    freeFormAddressLine
                                    contactMethodTypes
                            }
                            billingAddress {
                                    primary
                                    freeFormAddressLine
                                    contactMethodTypes
                            }
                            ... on IndividualClient {
                              displayName
                              firstName
                              lastName
                            }
                            ... on OrganizationClient {
                              displayName
                              firstName
                              lastName
                              organizationName
                            }
                        }        
                    }
                  }
                }";

            var request = new GraphQLHttpRequest {
                Query = query
            };

            var clientQueryResponse = await SendAsync<ClientQueryResponse>(request).ConfigureAwait(false);

            return clientQueryResponse.Firm.Clients.Nodes;
        }

        internal async Task<Client> GetClientByDisplayNameAsync(string displayName)
        {
            string filter = $"(filter: {{displayName: {{ equals: \"{displayName}\"}}}})";
            List<Client> clients = await GetClientsAsync(filter).ConfigureAwait(false);

            return clients.FirstOrDefault();
        }

        internal async Task<Client> GetClientByIdAsync(string id)
        {
            string filter = $"(filter: {{id: {{ equals: \"{id}\"}}}})";
            List<Client> clients = await GetClientsAsync(filter).ConfigureAwait(false);

            return clients.FirstOrDefault();
        }

        internal async Task<Client> CreateOrganizationalClient(Client client)
        {
            string mutation = @"
              mutation createOrganizationClient($input_0: CreateOrganizationClientInput!) {
                createOrganizationClient(client: $input_0) {
                  id
                  displayName
                  firstName
                  lastName
                  email {
                    address
                    contactMethodTypes
                  }
                }
              }";

            GraphQLHttpRequest request = BuildMutationRequest(client, mutation, "createOrganizationClient");

            var response = await SendAsync<CreateOrganizationClientResponse>(request).ConfigureAwait(false);
            return response?.Client;
        }

        internal async Task<Client> UpdateOrganizationalClient(Client client)
        {
            string mutation = @"
              mutation updateOrganizationClient($input_0: UpdateOrganizationClientInput!) {
                updateOrganizationClient(client: $input_0) {
                  id
                  active
                  displayName
                  firstName
                  lastName
                  email {
                    address
                    contactMethodTypes
                  }
                }
              }";

            GraphQLHttpRequest request = BuildMutationRequest(client, mutation, "updateOrganizationClient");

            var response = await SendAsync<UpdateOrganizationClientResponse>(request).ConfigureAwait(false);
            return response?.Client;
        }

        private GraphQLHttpRequest BuildMutationRequest(Client client, string mutation, string operation)
        {
            string serializedClient = JsonConvert.SerializeObject(
                client,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );

            string variables = $"{{ \"input_0\": {serializedClient} }}";

            return new GraphQLHttpRequest
            {
                Query = mutation,
                OperationName = operation,
                Variables = variables
            };
        }

        private async Task<T> SendAsync<T>(GraphQLHttpRequest request)
        {
            GraphQLResponse<object> response = await _graphQlClient.SendQueryAsync<object>(request).ConfigureAwait(false);

            if (response.Errors?.Length > 0)
            {
                throw new AggregateException(response.Errors.Select(e => new Exception(e.Message)));
            }

            return JsonConvert.DeserializeObject<T>(response.Data.ToString());
        }
    }
}
