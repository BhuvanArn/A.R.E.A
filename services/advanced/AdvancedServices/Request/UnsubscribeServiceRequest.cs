﻿using EventBus.Event;

namespace AdvancedServices.Request;

public class UnsubscribeServiceRequest
{
    public string Name { get; set; }

    public Credentials Credentials { get; set; }
}