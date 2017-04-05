namespace Goop.Wpf
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    public class RoutedEventUtilities
    {
        public RoutedEventUtilities(Type ownerType)
        {
            this.OwnerType = ownerType;
        }

        public Type OwnerType { get; }

        public RoutedEvent Register<T>(string name, RoutingStrategy routingStrategy)
        {
            ValidateEvent<T>(name);
            return EventManager.RegisterRoutedEvent(name, routingStrategy, typeof(T), OwnerType);
        }

        public RoutedEvent RegisterBubble<T>(string name)
        {
            return Register<T>(name, RoutingStrategy.Bubble);
        }

        public RoutedEvent RegisterDirect<T>(string name)
        {
            return Register<T>(name, RoutingStrategy.Direct);
        }

        public RoutedEvent RegisterTunnel<T>(string name)
        {
            return Register<T>(name, RoutingStrategy.Tunnel);
        }

        [Conditional("DEBUG")]
        private void ValidateEvent<T>(string name)
        {
            var eventInfo = OwnerType.GetEvent(name);
            if (eventInfo == null)
            {
                Debug.Fail($"Event `{OwnerType.Name}.{name}` does not exist.");
            }
            else
            {
                Debug.Assert(
                    eventInfo.EventHandlerType == typeof(T),
                    $"Event `{OwnerType.Name}.{name}` is registered with incorrect type.",
                    $"Expected `{eventInfo.EventHandlerType.Name}` but was `{typeof(T)}`.");
            }
        }
    }

    public static class RoutedEventUtilities<TOwner>
    {
        private static readonly Type OwnerType = typeof(TOwner);

        private static readonly RoutedEventUtilities Helper = new RoutedEventUtilities(OwnerType);

        public static RoutedEvent Register<T>(string name, RoutingStrategy routingStrategy)
        {
            return Helper.Register<T>(name, routingStrategy);
        }

        public static RoutedEvent RegisterBubble<T>(string name)
        {
            return Helper.Register<T>(name, RoutingStrategy.Bubble);
        }

        public static RoutedEvent RegisterDirect<T>(string name)
        {
            return Helper.Register<T>(name, RoutingStrategy.Direct);
        }

        public static RoutedEvent RegisterTunnel<T>(string name)
        {
            return Helper.Register<T>(name, RoutingStrategy.Tunnel);
        }
    }
}