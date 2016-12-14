﻿//-----------------------------------------------------------
// <copyright file="synchDAL.cs" company="adidas AG">
// Copyright (C) 2016 adidas AG.
// </copyright>
//-----------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using adidas.clb.MobileApproval.Exceptions;
using adidas.clb.MobileApproval.Models;
using adidas.clb.MobileApproval.Utility;

namespace adidas.clb.MobileApproval.App_Code.DAL.Synch
{
    /// <summary>
    /// The class which implements methods for data access layer of synch.
    /// </summary> 
    public class SynchDAL
    {
        /// <summary>
        /// method to get all requests per user
        /// </summary>
        /// <param name="UserID">takes userid as input</param>
        /// <returns>returns list of requests associated to user</returns>
        public List<RequestEntity> GetUserRequests(string UserID,string requeststatus)
        {
            try
            {
                //partionkey filter
                string partitionkeyfilter=TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.PartitionKey, QueryComparisons.Equal, string.Concat(CoreConstants.AzureTables.RequestsPK, UserID));
                //request status filter
                string statusfilter=TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.Status, QueryComparisons.Equal, requeststatus);
                //generate query to get all requests per user based on filter conditions
                TableQuery<RequestEntity> query = new TableQuery<RequestEntity>().Where(TableQuery.CombineFilters(partitionkeyfilter,TableOperators.And,statusfilter));
                //call dataprovider method to get entities from azure table
                List<RequestEntity> allrequests = DataProvider.GetEntitiesList<RequestEntity>(CoreConstants.AzureTables.RequestTransactions, query);
                return allrequests;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving requests per user from RequestTransactions azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to get requests per userbackend
        /// </summary>
        /// <param name="UserID">takes userid as input</param>
        /// <param name="backendID">takes backendid as input</param>
        /// <returns>returns list of requets associated userbackend</returns>
        public List<RequestEntity> GetUserBackendRequests(string UserID, string backendID, string requeststatus)
        {
            try
            {
                string partitionkeyfilter = TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.PartitionKey, QueryComparisons.Equal, string.Concat(CoreConstants.AzureTables.RequestsPK, UserID));
                string backendfilter = TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.BackendId, QueryComparisons.Equal, backendID);
                //combine partionkey filter with backend filter 
                string combinefiletr = TableQuery.CombineFilters(partitionkeyfilter,TableOperators.And,backendfilter);
                //request status filter
                string statusfilter = TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.Status, QueryComparisons.Equal, requeststatus);
                //final filter to get requests based on partitionkey, backendid, request status
                string finalfilter = TableQuery.CombineFilters(combinefiletr, TableOperators.And, statusfilter);
                //generate query to get all user associated requests
                TableQuery<RequestEntity> query = new TableQuery<RequestEntity>().Where(finalfilter);
                //call dataprovider method to get entities from azure table
                List<RequestEntity> allrequests = DataProvider.GetEntitiesList<RequestEntity>(CoreConstants.AzureTables.RequestTransactions, query);
                return allrequests;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving requests per userbackend from RequestTransactions azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to get request
        /// </summary>
        /// <param name="requestID">takes requestid as input</param>
        /// <returns>returns request</returns>
        public RequestEntity GetRequest(string userID, string requestID)
        {
            try
            {
                //call dataprovider method to get entities from azure table
                RequestEntity request = DataProvider.Retrieveentity<RequestEntity>(CoreConstants.AzureTables.RequestTransactions, string.Concat(CoreConstants.AzureTables.RequestsPK, userID), requestID);
                return request;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving request from RequestTransactions azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }
        
        /// <summary>
        /// method to get approvers per request
        /// </summary>
        /// <param name="requestID">takes requestid as input</param>
        /// <returns>returns list of approvers per request</returns>
        public List<ApproverEntity> GetApprovers(string requestID)
        {
            try
            {
                //generate query to get all approvers associated request
                TableQuery<ApproverEntity> query = new TableQuery<ApproverEntity>().Where(TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.PartitionKey, QueryComparisons.Equal, string.Concat(CoreConstants.AzureTables.ApproverPK, requestID)));
                //call dataprovider method to get entities from azure table
                List<ApproverEntity> approvers = DataProvider.GetEntitiesList<ApproverEntity>(CoreConstants.AzureTables.RequestTransactions, query);
                return approvers;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving approvers from RequestTransactions azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to get fields associated to request
        /// </summary>
        /// <param name="requestID">takes requestid as input</param>
        /// <returns>returns list of fields per request</returns>
        public List<FieldEntity> GetFields(string requestID)
        {
            try
            {
                //generate query to get all fields associated request
                TableQuery<FieldEntity> query = new TableQuery<FieldEntity>().Where(TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.PartitionKey, QueryComparisons.Equal, string.Concat(CoreConstants.AzureTables.FieldPK, requestID)));
                //call dataprovider method to get entities from azure table
                List<FieldEntity> fields = DataProvider.GetEntitiesList<FieldEntity>(CoreConstants.AzureTables.RequestTransactions, query);
                return fields;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving fields from RequestTransactions azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to get user backend synch
        /// </summary>
        /// <param name="UserID">takes user id as input</param>
        /// /// <param name="UserBackendID">takes user backend id as input</param>
        /// <returns>returns backend synch entity for user</returns>
        public SynchEntity GetUserBackendSynch(string UserID, string UserBackendID)
        {
            try
            {
                //call dataprovider method to get entities from azure table
                SynchEntity synchentity = DataProvider.Retrieveentity<SynchEntity>(CoreConstants.AzureTables.UserDeviceConfiguration, (string.Concat(CoreConstants.AzureTables.BackendSynchPK, UserID)), UserBackendID);
                return synchentity;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving userbackendsynch from userdeviceconfig azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to get request synch
        /// </summary>
        /// <param name="requestsynchpk">takes request synch partitionkey as input</param>
        /// /// <param name="requestid">takes request id as input</param>
        /// <returns>returns backend synch entity for user</returns>
        public RequestSynchEntity GetRequestSynch(string requestsynchpk, string requestid)
        {
            try
            {
                //call dataprovider method to get entities from azure table
                RequestSynchEntity synchentity = DataProvider.Retrieveentity<RequestSynchEntity>(CoreConstants.AzureTables.UserDeviceConfiguration, requestsynchpk, requestid);
                return synchentity;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving request synch from userdeviceconfig azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to add/upadte Request entity to azure table
        /// </summary>
        /// <param name="synch">takes userbackend synch entity as input</param>
        public void AddUpdateBackendSynch(SynchEntity synch)
        {
            try
            {
                //call dataprovider method to insert entity into azure table
                DataProvider.InsertReplaceEntity<SynchEntity>(CoreConstants.AzureTables.RequestTransactions, synch);
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while inserting userbackend synch into requestTransactions azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to add/upadte Request synch entity to azure table
        /// </summary>
        /// <param name="synch">takes request synch entity as input</param>
        public void AddUpdateRequestSynch(RequestSynchEntity synch)
        {
            try
            {
                //call dataprovider method to insert entity into azure table
                DataProvider.InsertReplaceEntity<RequestSynchEntity>(CoreConstants.AzureTables.RequestTransactions, synch);
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while inserting request synch into requestTransactions azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }

        /// <summary>
        /// method to get userbackends
        /// </summary>
        /// <param name="UserID">takes userid as input</param>
        /// <returns> returns list of user backends associated to user</returns>
        public List<UserBackendEntity> GetUserAllBackends(string UserID, List<string> userbackendidslist)
        {
            try
            {
                string finalfilter = string.Empty;
                //partionkey filter
                string partitionkeyfilter = TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.PartitionKey, QueryComparisons.Equal, string.Concat(CoreConstants.AzureTables.UserBackendPK, UserID));
                //loop through each userbackend to generate rowkey filter for each one
                foreach (string userbackendid in userbackendidslist)
                {
                    string rowkeyfilter = TableQuery.GenerateFilterCondition(CoreConstants.AzureTables.RowKey, QueryComparisons.Equal, userbackendid);
                    //combine partitionkey filter with rowkey to get each entity
                    string currentrowfilter = TableQuery.CombineFilters(partitionkeyfilter, TableOperators.And, rowkeyfilter);
                    //if it is at first postion, no need to add OR condotion
                    if ((userbackendidslist.First() == userbackendid))
                    {
                        finalfilter = currentrowfilter;
                    }
                    else
                    {
                        finalfilter = TableQuery.CombineFilters(finalfilter, TableOperators.Or, currentrowfilter);
                    }
                }
                //generate query to get all user associated backends
                TableQuery<UserBackendEntity> query = new TableQuery<UserBackendEntity>().Where(finalfilter);
                List <UserBackendEntity> alluserbackends = DataProvider.GetEntitiesList<UserBackendEntity>(CoreConstants.AzureTables.UserDeviceConfiguration, query);
                return alluserbackends;
            }
            catch (Exception exception)
            {
                LoggerHelper.WriteToLog(exception + " - Error while retrieving userbackends from userdeviceconfig azure table in DAL : "
                      + exception.ToString(), CoreConstants.Priority.High, CoreConstants.Category.Error);
                throw new DataAccessException();
            }
        }
    }
}