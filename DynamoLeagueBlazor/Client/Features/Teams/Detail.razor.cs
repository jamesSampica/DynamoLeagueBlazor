﻿using DynamoLeagueBlazor.Shared.Features.Teams;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace DynamoLeagueBlazor.Client.Features.Teams;

public partial class Detail
{
    [Inject] private HttpClient HttpClient { get; set; } = null!;

    [Parameter] public int TeamId { get; set; }

    private GetTeamDetailResult? _result;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _result = await HttpClient.GetFromJsonAsync<GetTeamDetailResult>($"teams/{TeamId}");
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
    }
}
