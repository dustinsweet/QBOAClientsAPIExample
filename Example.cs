// *******************************************************************************
// <copyright file="Example.cs" company="Intuit">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientsApiExample.Model;

namespace Intuit.ClientsApiExample
{
    internal class Example
    {
        private ApiClient _apiClient;

        internal Example()
        {
            _apiClient = new ApiClient();
        }

        internal async Task RunAsync()
        {
            // EXAMPLE 1 of 5: Create a new organization client
            Client createdClient = await CreateClientAsync().ConfigureAwait(false);

            // EXAMPLE 2 of 5: Retrieve the newly created client
            Client retrievedClient = await GetClientByDisplayNameAsync(createdClient).ConfigureAwait(false);

            // EXAMPLE 3 of 5: Update the client
            Client updatedClient = await UpdateClientAsync(retrievedClient).ConfigureAwait(false);

            // EXAMPLE 4 of 5: Retrieve the newly updated client (this time by id), to confirm our changes took
            await GetClientByIdAsync(updatedClient).ConfigureAwait(false);

            // NOTE: Creating and updating an *individual* client is a similar process (as compared to an organizational client),
            //  and for that reason isn't included in these examples. For an individual client, the mutation operation names
            //  are different, and also the FirstName, LastName, and Email fields are requred.  Whereas fo an organizational client,
            //  only OrganizationName and Email are required.  See API docs for specifics.

            // EXAMPLE 5 of 5: Retrieve all clients and print some info to the console window
            await GetClientsAsync().ConfigureAwait(false);
        }

        private async Task GetClientsAsync()
        {
            List<Client> clients = await _apiClient.GetClientsAsync().ConfigureAwait(false);

            int displayNameWidth = clients.OrderByDescending(c => c.DisplayName.Length).First().DisplayName.Length + 2;
            int emailWidth = clients.OrderByDescending(c => c.Email.Address.Length).First().Email.Address.Length + 2;
            int orgNameWidth = clients.OrderByDescending(c => c.OrganizationName?.Length).First().OrganizationName.Length + 2;
            int activeWidth = 9;

            Console.WriteLine("\nAll clients:\n");
            Console.WriteLine(String.Format(
                    $"  {{0,-{displayNameWidth}}}{{1,-{emailWidth}}}{{2,-{orgNameWidth}}}{{3,-7}}",
                    "DISPLAY NAME", "EMAIL", "ORG NAME", "ACTIVE")
            );
            Console.WriteLine(new string('-', displayNameWidth + emailWidth + orgNameWidth + activeWidth));

            foreach (Client client in clients)
            {
                Console.WriteLine(String.Format(
                    $"  {{0,-{displayNameWidth}}}{{1,-{emailWidth}}}{{2,-{orgNameWidth}}}{{3,-{activeWidth}}}",
                    client.DisplayName,
                    client.Email.Address,
                    client.OrganizationName is null ? "<null>" : client.OrganizationName,
                    client.Active)
                );
            }
        }

        private async Task<Client> CreateClientAsync()
        {
            var clientToCreate = new Client
            {
                Email = new Email
                {
                    Address = "tony.stark@anymail.com",
                },
                DisplayName = "Tony Star",
                FirstName = "Tony",
                LastName = "Stark",
                OrganizationName = "The Avengers",
                BillingAddress = new BillingAddress
                {
                    Primary = true,
                    ContactMethodTypes = new List<string> { "BILLING", "SHIPPING" },
                    FreeFormAddressLine = "10880 Malibu Point, Malibu CA, 90265"
                }
            };

            var createdClient = await _apiClient.CreateOrganizationalClient(clientToCreate).ConfigureAwait(false);

            Console.WriteLine($"Client ID:{createdClient.Id} created successfully (Display Name='{createdClient.DisplayName}').");

            return createdClient;
        }

        private async Task<Client> GetClientByDisplayNameAsync(Client client)
        {
            var retrievedClient = await _apiClient.GetClientByDisplayNameAsync(client.DisplayName).ConfigureAwait(false);

            Console.WriteLine($"Client ID:{retrievedClient.Id} retrieved successfully (Display Name='{retrievedClient.DisplayName}').");

            return retrievedClient;
        }

        private async Task<Client> GetClientByIdAsync(Client client)
        {
            var retrievedClient = await _apiClient.GetClientByIdAsync(client.Id).ConfigureAwait(false);

            Console.WriteLine($"Client ID:{retrievedClient.Id} retrieved successfully (Display Name='{retrievedClient.DisplayName}').");

            return retrievedClient;
        }

        private async Task<Client> UpdateClientAsync(Client client)
        {
            // Oops, we misspelled Tony's last name - let's fix it
            client.DisplayName = "Tony Stark";
            client.LastName = "Stark";
            client.OrganizationName = "The Avengers";

            var updatedClient = await _apiClient.UpdateOrganizationalClient(client).ConfigureAwait(false);

            Console.WriteLine($"Client ID:{updatedClient.Id} updated successfully (Display Name='{updatedClient.DisplayName}').");

            return updatedClient;
        }

    }
}
