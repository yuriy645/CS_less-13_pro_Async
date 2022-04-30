## My homework for the C# Professional course, less-13_pro_Async
1. Create a WPF application. On the main form of the application, place 3 buttons with
    names: IsComplete, End, Callback. Organize button click handlers like this
    so that they initiate the asynchronous execution of some method (method
    determine for yourself, you can use something like Add or the more abstract Compute).
    For each of the buttons, the completion of the async method should be tracked accordingly:
- IsComplete - using the value of the IsComplete property
- End - just by applying EndInvoke
- Callback - using the callback method
2. Create a console application in which organize an asynchronous method call.
    Using the BeginInvoke construct, pass some information to the thread (perhaps in
    string format). Organize the processing of the transferred data in the callback method.
