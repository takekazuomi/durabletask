﻿//  ----------------------------------------------------------------------------------
//  Copyright Microsoft Corporation
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  ----------------------------------------------------------------------------------

namespace DurableTask
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DurableTask.Tracking;

    /// <summary>
    /// Instance Store provider interface to allow storage and lookup for orchestration state and event history
    /// </summary>
    public interface IOrchestrationServiceInstanceStore
    {
        /// <summary>
        /// Gets the maximum length a history entry can be so it can be truncated if neccesary
        /// </summary>
        /// <returns>The maximum length</returns>
        int MaxHistoryEntryLength { get; }

        /// <summary>
        /// Runs initialization to prepare the storage for use
        /// </summary>
        /// <param name="recreateStorage">Flag to indicate whether the storage should be recreated.</param>
        Task InitializeStorageAsync(bool recreateStorage);

        /// <summary>
        /// Deletes instances storage
        /// </summary>
        Task DeleteStorageAsync();

        /// <summary>
        /// Writes a list of history events to storage
        /// </summary>
        /// <param name="entities">List of history events to write</param>
        Task<object> WriteEntitesAsync(IEnumerable<OrchestrationHistoryEvent> entities);

        /// <summary>
        /// Deletes a list of history events from storage
        /// </summary>
        /// <param name="entities">List of history events to delete</param>
        Task<object> DeleteEntitesAsync(IEnumerable<OrchestrationHistoryEvent> entities);

        /// <summary>
        /// Gets a list of orchestration states for a given instance
        /// </summary>
        /// <param name="instanceId">The instance id to return state for</param>
        /// <param name="allInstances">Flag indiciation whether to get all history execution ids or just the most recent</param>
        /// <returns>List of matching orchestration states</returns>
        Task<IEnumerable<OrchestrationStateHistoryEvent>> GetOrchestrationStateAsync(string instanceId, bool allInstances);

        /// <summary>
        /// Gets the orchestration state for a given instance and execution id
        /// </summary>
        /// <param name="instanceId">The instance id to return state for</param>
        /// <param name="executionId">The execution id to return state for</param>
        /// <returns>The matching orchestation state or null if not found</returns>
        Task<OrchestrationStateHistoryEvent> GetOrchestrationStateAsync(string instanceId, string executionId);

        /// <summary>
        /// Gets the list of history events for a given instance and execution id
        /// </summary>
        /// <param name="instanceId">The instance id to return history for</param>
        /// <param name="executionId">The execution id to return history for</param>
        /// <returns>List of history events</returns>
        Task<IEnumerable<OrchestrationWorkItemEvent>> GetOrchestrationHistoryEventsAsync(string instanceId, string executionId);

        /// <summary>
        /// Purges history from storage for given time range
        /// </summary>
        /// <param name="thresholdDateTimeUtc">The datetime in UTC to use as the threshold for purging history</param>
        /// <param name="timeRangeFilterType">What to compare the threshold date time against</param>
        /// <returns>The number of history events purged.</returns>
        Task<int> PurgeOrchestrationHistoryEventsAsync(DateTime thresholdDateTimeUtc, OrchestrationStateTimeRangeFilterType timeRangeFilterType);
    }
}