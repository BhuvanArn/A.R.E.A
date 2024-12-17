﻿using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class GetActionsReactionsEventHandler : IIntegrationEventHandler<GetActionsReactionsEvent, (GetActionsReactionsEventHandler.ActionsReactionsResponse, ResultType)>
{
    private readonly ServiceService _serviceService;
    private readonly ActionService _actionService;
    private readonly ReactionService _reactionService;

    public GetActionsReactionsEventHandler(ActionService actionService, ReactionService reactionService, ServiceService serviceService)
    {
        _actionService = actionService;
        _reactionService = reactionService;
        _serviceService = serviceService;
    }
    
    public async Task<(ActionsReactionsResponse, ResultType)> HandleAsync(GetActionsReactionsEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var service = (await _serviceService.FindServicesAsync(s => s.UserId == userId && s.Name == @event.ServiceName)).FirstOrDefault();

        if (service is null)
        {
            return (new(), ResultType.Fail);
        }
        
        var actions = await _actionService.FindActionsAsync(a => a.ServiceId == service.Id);
        var reactions = await _reactionService.FindReactionsAsync(r => r.ServiceId == service.Id);

        var response = new ActionsReactionsResponse
        {
            Actions = actions.Select(a => new ActionDto
            {
                Name = a.Name,
                Description = a.TriggerConfig
            }).ToList(),
            Reactions = reactions.Select(r => new ReactionDto
            {
                Name = r.Name,
                Action = r.Action.Name,
                Description = r.ExecutionConfig
            }).ToList()
        };
    
        return (response, ResultType.Success);
    }
    
    public class ActionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ReactionDto
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }

    public class ActionsReactionsResponse
    {
        public List<ActionDto> Actions { get; set; }
        public List<ReactionDto> Reactions { get; set; }
    }
}