﻿namespace NServiceBus.AcceptanceTests.Performance.MessageDurability
{
    using System;
    using AcceptanceTesting;
    using EndpointTemplates;
    using FakeTransport;
    using NUnit.Framework;

    public class When_disabling_durable_messages_on_durable_transport : NServiceBusAcceptanceTest
    {
        [Test]
        public void Should_throw_exception_at_startup()
        {
            var exception = Assert.ThrowsAsync<AggregateException>(() => Scenario.Define<Context>()
                .WithEndpoint<EndpointDisablingDurableMessages>(c => c
                    .When(e => e.SendLocal(new RegularMessage())))
                .Done(c => c.EndpointsStarted)
                .Run());

            Assert.That(exception.InnerException.InnerException, Is.TypeOf<Exception>());
            Assert.That(exception.InnerException.InnerException.Message, Does.Contain("The configured transport does not support non durable messages but you have configured some messages to be non durable"));
        }

        class Context : ScenarioContext
        {
        }

        class EndpointDisablingDurableMessages : EndpointConfigurationBuilder
        {
            public EndpointDisablingDurableMessages()
            {
                EndpointSetup<DefaultServer>(c =>
                {
                    c.DisableDurableMessages();
                    c.UseTransport<FakeTransport>();
                });
            }
        }

        class RegularMessage : ICommand
        {
        }
    }
}