using Akka.Actor;
using Akka.Event;

namespace ActorModelDemo
{
    public class GreetActor : ReceiveActor
    {
        private readonly ILoggingAdapter __log = Context.GetLogger();
        private readonly string __greeting;

        public GreetActor(string greeting)
        {
            this.__greeting = greeting;
            Receive<string>((name) =>
            {
                string greetMessage = $"{this.__greeting} {name}!";
                __log.Info(greetMessage);

                Sender.Tell(greetMessage);
            });
        }

        public static Props Props(string greeting)
        {
            return Akka.Actor.Props.Create(() => new GreetActor(greeting));
        }
    }
}
