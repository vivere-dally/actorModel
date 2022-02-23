using Akka.Actor;

namespace ASPAkka
{
    public class UserActor : ReceiveActor
    {
        private readonly IDictionary<Guid, User> __users;

        public UserActor()
        {
            this.__users = new Dictionary<Guid, User>();
            Receive<CreateUser>((command) =>
            {
                var user = new User(Guid.NewGuid(), command.Name);
                this.__users[user.Id] = user;
            });

            Receive<GetUserById>((query) =>
            {
                if (this.__users.TryGetValue(query.Id, out var user))
                {
                    Sender.Tell(user);
                    return;
                }

                Sender.Tell("User not found");
            });

            Receive<GetUsers>((_) =>
            {
                Sender.Tell(this.__users.Values.ToList());
            });
        }

        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new UserActor());
        }

    }

    public delegate IActorRef UserActorProvider();
}
