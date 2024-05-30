using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class MyButton : Button
{
    protected override void OnPaint(PaintEventArgs e)
    {
        GraphicsPath path = new();
        int cornerRadius = 10; // Adjust the corner radius as desired

        path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90);
        path.AddArc(Width - (cornerRadius * 2), 0, cornerRadius * 2, cornerRadius * 2, 270, 90);
        path.AddArc(Width - (cornerRadius * 2), Height - (cornerRadius * 2), cornerRadius * 2, cornerRadius * 2, 0, 90);
        path.AddArc(0, Height - (cornerRadius * 2), cornerRadius * 2, cornerRadius * 2, 90, 90);
        path.CloseFigure();

        Region = new Region(path);

        base.OnPaint(e);
    }

    private Color defaultForeColor = ColorTranslator.FromHtml("#ffffff"); // Set your desired default ForeColor here

    public MyButton()
    {
        ForeColor = defaultForeColor;
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        this.ForeColor = defaultForeColor;
    }
}