using System;
using AcmeTube.Application.Core.Commons;
using MongoDB.Bson;

namespace AcmeTube.Infrastructure.Services;

/// <summary>
/// Generator of keys randomly generated.
/// </summary>
public class KeyGenerator : IKeyGenerator
{
    /// <summary>
    /// Generates random number.
    /// </summary>
    /// <returns></returns>
    public int GenerateNumber(int min, int max) => new Random().Next(min, max);

    /// <inheritdoc />
    // public string Generate() => SequentialGuid.NewGuid().ToString("N");
    public string Generate() => ObjectId.GenerateNewId().ToString().ToLower();
}