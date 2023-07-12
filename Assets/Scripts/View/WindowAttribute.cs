
public class ViewAttribute : System.Attribute
{

    public EView _view;

    public ViewAttribute(EView view)
    {
        _view = view;
    }

}
