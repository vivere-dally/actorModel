using Akka.Actor;

namespace ActorModelDemo
{
    public readonly record struct Deposit(int Value);
    public readonly record struct Withdraw(int Value);
    public readonly record struct InvalidWithdrawl(int Total, int Value);

    public class ATMActor : ReceiveActor
    {
        private int __total;

        public ATMActor(int total)
        {
            this.__total = total;
            Receive<Deposit>((d) =>
            {
                this.__total += d.Value;
                Sender.Tell(this.__total);
            });

            Receive<Withdraw>((d) =>
            {
                if (this.__total < d.Value)
                {
                    Sender.Tell(new InvalidWithdrawl(this.__total, d.Value));
                    return;
                }

                this.__total -= d.Value;
                Sender.Tell(this.__total);
            });
        }

        public static Props Props(int total)
        {
            return Akka.Actor.Props.Create(() => new ATMActor(total));
        }
    }
}
