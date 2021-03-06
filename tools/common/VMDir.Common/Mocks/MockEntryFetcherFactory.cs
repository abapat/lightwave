﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VmDirInterop.Schema;
using VmDirInterop.Schema.Interfaces;

//TODO - This file is for testing purposes to provide a mock server with test data. Will be removed.
namespace SchemaConnectionTest.Mocks
{
    public class MockEntryFetcherFactory : IEntryFetcherFactory
    {
        private SchemaConnTestDataLoader testDataLoader;

        public MockEntryFetcherFactory(SchemaConnTestDataLoader testDataLoader)
        {
            this.testDataLoader = testDataLoader;
        }

        public IEntryFetcher CreateEntryFetcher(String hostName, String upn, String passwd)
        {
            MockEntryFetcher entryFetcher = null;
            SchemaConnTestData testData = testDataLoader.GetTestData(hostName);
            if (testData != null && testData.Reachable)
            {
                entryFetcher = new MockEntryFetcher(testData);
            }
            return entryFetcher;
        }
    }
}
