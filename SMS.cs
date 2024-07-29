using System;
using System.Text;
using System.Windows.Forms;

namespace Tarea_Extraclase_1;


    public partial class Form1 : Form
    {
        private Label MensajeL;
        private Label PuertoDestinoL;
        private Label PuertoLocalL;
        private Label MensajesEntrantesL;
        private TextBox EscribirMensajeT;
        private TextBox PuertoDestinoT;
        private TextBox PuertoLocalT;
        private TextBox MensajesEntrantesT;
        private Button EnviarB;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Form1()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
            InitializeComponent();
            IniciarInterfaz();
        }
    

        private void IniciarInterfaz()
        {
            //Labels de Interfaz

            MensajeL = new Label 
            {
                Text = "Mensaje:",
                Location = new Point(20,20),
                AutoSize = true           
            };
            this.Controls.Add(MensajeL);

            PuertoDestinoL = new Label
            {
                Text = "Puerto Destino:",
                Location = new Point(20,60),
                AutoSize = true
            };
            this.Controls.Add(PuertoDestinoL);

            PuertoLocalL = new Label
            {
                Text = "Puerto Local:",
                Location = new Point(210,60),
                AutoSize = true
            };
            this.Controls.Add(PuertoLocalL);

            MensajesEntrantesL = new Label
            {
                Text = "Mensajes Entrantes",
                Location = new Point(20,140),
                AutoSize = true
            };
            this.Controls.Add(MensajesEntrantesL);

            //Textbox de la interfaz

            EscribirMensajeT = new TextBox
            {
                Width = 300,
                Location = new Point(80,15)
            };
            this.Controls.Add(EscribirMensajeT);

            PuertoDestinoT = new TextBox
            {
                Width = 70,
                Location = new Point(110, 55)
            };
            this.Controls.Add(PuertoDestinoT);

            PuertoLocalT = new TextBox
            {
                Width = 70,
                Location = new Point(290, 55),
                ReadOnly = true
            };
            this.Controls.Add(PuertoLocalT);

            MensajesEntrantesT = new TextBox
            {
                Width = 360,
                Height = 270,
                Location = new Point(20,160),
                ReadOnly = true,
                Multiline = true
            };
            this.Controls.Add(MensajesEntrantesT);

            EnviarB = new Button
            {
                Text = "Enviar",
                Location = new Point(160,100)
            };
            this.Controls.Add(EnviarB);
        }
    }