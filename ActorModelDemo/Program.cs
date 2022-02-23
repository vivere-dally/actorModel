// See https://aka.ms/new-console-template for more information
using ActorModelDemo;
using Akka.Actor;

ActorSystem actorSystem = ActorSystem.Create("ActorModelDemo");

//IActorRef greetActor = actorSystem.ActorOf(GreetActor.Props("Hello"), "greet");
//Console.WriteLine(await greetActor.Ask("World"));

IActorRef atmActor = actorSystem.ActorOf(ATMActor.Props(10), "atmactor");

var rnd = new Random();
while (true)
{
    int value = rnd.Next(10);
    if (rnd.Next(2) == 0)
    {
        var total = await atmActor.Ask(new Deposit(value));
        Console.WriteLine($"Deposited {value}. Total {total}");
        continue;
    }

    var result = await atmActor.Ask(new Withdraw(value));
    if (result is InvalidWithdrawl)
    {
        var iw = (InvalidWithdrawl)result;
        Console.WriteLine($"Cannot withdraw {iw.Value}. Total is only {iw.Total}");
        break;
    }

    Console.WriteLine($"Withdrawn {value}. Total {result}");
    Thread.Sleep(100);
}

await actorSystem.Terminate();
