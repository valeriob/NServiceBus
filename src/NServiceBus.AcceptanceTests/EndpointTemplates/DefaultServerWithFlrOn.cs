﻿namespace NServiceBus.AcceptanceTests.EndpointTemplates
{
    class DefaultServerWithFlrOn : DefaultServer
    {
        protected override bool DisableFLR()
        {
            return false;
        }
    }
}