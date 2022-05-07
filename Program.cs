using Moq;

var pro = new Mock<IProducer>();
pro.SetupAdd(m => m.OnProduced += It.IsAny<EventHandler<MyEvent>>());
pro.Setup(x => x.Produce()).Callback(() => { pro.Raise(m => m.OnProduced += null, pro.Object, new MyEvent("rat")); });
var consumer = new Consumer(pro.Object);
pro.Object.Produce();

//

public class MyEvent : EventArgs
{
    public MyEvent(string message)
    {
        Message = message;
    }

    public string Message { get; }
}

public class Consumer
{
    public Consumer(IProducer producer)
    {
        producer.OnProduced += msgHandler;
        //producer.Produce();
    }

    private static void msgHandler(object sender, MyEvent msgClass)
    {
        Console.WriteLine(msgClass.Message);
        Console.WriteLine("chcici");
    }
}

public interface IProducer
{
    event EventHandler<MyEvent> OnProduced;
    void Produce();
}

public class Producer : IProducer
{
    public event EventHandler<MyEvent> OnProduced;

    public void Produce()
    {
        var msg = new MyEvent("Hello guys");
        OnProduced?.Invoke(this, msg);
    }
}