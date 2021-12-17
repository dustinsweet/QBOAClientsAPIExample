// *******************************************************************************
// <copyright file="Program.cs" company="Intuit">
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
using System.Threading.Tasks;

namespace Intuit.ClientsApiExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
             * !!! STOP !!!: Before running this example code you MUST update the BEARER_TOKEN const value in the ApiClient class.
             * To obtain your bearer token you need to connect to your QBOA firm through OAuth2.  This can be done via
             * Intuit's OAuth 2.0 Playground (https://developer.intuit.com/app/developer/playground).  Various tools
             * support this flow as well (Insomnia, Postman, etc.).
             * 
             * ALSO NOTE: The bearer token is only valid for 1 hour.  You'll need to refresh it if you encounter
             * authentication errors while running this example code.
             * 
             * WARNING: THIS IS NOT PRODUCTION CODE!  Do not use it in your own production apps. The purpose is simply to show the CRUD operations
             * that are available on the QBOA Clients API. Suggest reading https://graphql.org/, as well as researching C#'s recommended
             * best practices for implementing GraphQL clients
            */

            try
            {
                var example = new Example();
                await example.RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.ToString()}");
            }
        }
    }
}
