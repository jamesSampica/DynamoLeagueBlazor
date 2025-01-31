﻿using DynamoLeagueBlazor.Server.Models;

namespace DynamoLeagueBlazor.Tests.Features.Fines;

public class StartSeasonTests : IntegrationTestBase
{
    private const string _endpoint = "api/admin/startseason";

    [Fact]
    public async Task GivenUnauthenticatedUser_ThenDoesNotAllowAccess()
    {
        var application = CreateUnauthenticatedApplication();

        var client = application.CreateClient();

        var response = await client.PostAsync(_endpoint, null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GivenAuthenticatedUser_ThenDoesNotAllowAccess()
    {
        var application = CreateUserAuthenticatedApplication();

        var client = application.CreateClient();

        var response = await client.PostAsync(_endpoint, null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GivenAuthenticatedAdmin_WhenAPlayerIsEligibleForFreeAgency_ThenSetsPlayerToFreeAgent()
    {
        var application = CreateAdminAuthenticatedApplication();
        var mockPlayer = CreateFakePlayer();
        mockPlayer.YearContractExpires = null;
        await application.AddAsync(mockPlayer);

        var client = application.CreateClient();

        var result = await client.PostAsync(_endpoint, null);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var player = await application.FirstOrDefaultAsync<Player>();
        player.Should().NotBeNull();
        player!.EndOfFreeAgency.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenAuthenticatedAdmin_WhenAPlayerIsNotEligibleForFreeAgency_ThenSkipsThatPlayer()
    {
        var application = CreateAdminAuthenticatedApplication();
        var mockPlayer = CreateFakePlayer();
        mockPlayer.YearContractExpires = int.MaxValue;
        mockPlayer.EndOfFreeAgency = null;
        await application.AddAsync(mockPlayer);

        var client = application.CreateClient();

        var result = await client.PostAsync(_endpoint, null);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var player = await application.FirstOrDefaultAsync<Player>();
        player.Should().NotBeNull();
        player!.EndOfFreeAgency.Should().BeNull();
    }
}
