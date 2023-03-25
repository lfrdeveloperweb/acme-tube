﻿using System.Collections.Generic;
using System.Linq;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Queries;
using AcmeTube.Domain.Commons;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AcmeTube.Application.DataContracts
{
    namespace Requests
    {

    }

    namespace Responses
    {
        /// <summary>
        /// The base class to be used as response from a method execution.
        /// </summary>
        public record Response
        {
            public Response() { }

            public Response(int statusCode, params Report[] reports)
            {
                StatusCode = statusCode;
                Reports = reports;
            }

            [JsonIgnore]
            public int StatusCode { get; init; }

            public Report[] Reports { get; init; }

            [JsonIgnore]
            public bool IsSuccessStatusCode => StatusCode is >= StatusCodes.Status200OK and < StatusCodes.Status300MultipleChoices;
            
            /// <summary>
            /// Create instance of <see cref="Response"/> with property State "<see cref="StatusCodes.Status200OK"/>".
            /// </summary>
            public static Response Ok() => new(StatusCodes.Status200OK);

            /// <summary>
            /// Create instance of <see cref="Response"/>.
            /// </summary>
            public static Response<T> Ok<T>(T data) => new(data, StatusCodes.Status200OK);

            public static Response<TResponseData> From<TModel, TResponseData>(QueryResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<TResponseData>(result.Data), result.StatusCode, result.Reports?.ToArray());

            public static ListResponse<TResponseData> From<TModel, TResponseData>(ListQueryResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<IEnumerable<TResponseData>>(result.Data), result.StatusCode, result.Reports?.ToArray());

            public static PaginatedResponse<TResponseData> From<TModel, TResponseData>(PaginatedQueryResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<IEnumerable<TResponseData>>(result.Data), result.PagingInfo, result.StatusCode, result.Reports?.ToArray());

            public static Response From(CommandResult result) => new(result.StatusCode, result.Reports?.ToArray());

            public static Response<TResponseData> From<TModel, TResponseData>(CommandResult<TModel> result, IMapper mapper) =>
                new(mapper.Map<TResponseData>(result.Data), result.StatusCode, result.Reports?.ToArray());
        }

        /// <summary>
        /// Represents the return of an internal operation.
        /// </summary>
        /// <typeparam name="T">Type of data to be returned.</typeparam>
        public record Response<T> : Response
        {
            public Response() { }

            public Response(T data, int statusCode, params Report[] reports) : base(statusCode, reports)
            {
                Data = data;
            }

            public T Data { get; init; }
        }

        public record ListResponse<TData> : Response<IEnumerable<TData>>
        {
            public ListResponse() { }

            public ListResponse(IEnumerable<TData> data, int statusCode, params Report[] reports)
                : base(data, statusCode, reports) { }
        }

        public record PaginatedResponse<TData> : Response<IEnumerable<TData>>
        {
            public PaginatedResponse() { }

            public PaginatedResponse(IEnumerable<TData> data, PagingInfo pagingInfo, int statusCode, params Report[] reports)
                : base(data, statusCode, reports)
            {
                PagingInfo = pagingInfo;
            }

            public PagingInfo PagingInfo { get; init; }
        }

        /// <summary>
        /// Representation of a domain response.
        /// </summary>
        /// <param name="Id">Identifier of domain.</param>
        /// <param name="Name">Name</param>
        public record IdentityNamedResponse(string Id, string Name);
    }
}
