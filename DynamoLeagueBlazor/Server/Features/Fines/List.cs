﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DynamoLeagueBlazor.Server.Infrastructure;
using DynamoLeagueBlazor.Server.Models;
using DynamoLeagueBlazor.Shared.Features.Fines;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamoLeagueBlazor.Server.Features.Fines;

[Authorize]
[ApiController]
[Route("fines")]
public class ListController : ControllerBase
{
    private readonly IMediator _mediator;

    public ListController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<FineListResult> GetAsync()
    {
        return await _mediator.Send(new ListQuery());
    }
}

public class ListQuery : IRequest<FineListResult> { }

public class ListHandler : IRequestHandler<ListQuery, FineListResult>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ListHandler(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<FineListResult> Handle(ListQuery request, CancellationToken cancellationToken)
    {
        var fines = await _dbContext.Fines
            .Include(p => p.Player)
            .ProjectTo<FineListResult.FineItem>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new FineListResult
        {
            Fines = fines
        };
    }
}

public class ListMappingProfile : Profile
{
    public ListMappingProfile()
    {
        CreateMap<Fine, FineListResult.FineItem>()
            .ForMember(d => d.FineStatus, mo => mo.MapFrom(s => s.Status ? "Approved" : "Pending"))
            .ForMember(d => d.PlayerName, mo => mo.MapFrom(s => s.Player.Name))
            .ForMember(d => d.PlayerHeadShotUrl, mo => mo.MapFrom(s => s.Player.HeadShot))
            .ForMember(d => d.FineAmount, mo => mo.MapFrom(s => s.FineAmount.ToString("C2")));
    }
}