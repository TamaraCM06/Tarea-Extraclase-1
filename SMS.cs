using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Tarea_Extraclase_1;

//  Clase principal - define que se trabaja de modo winforms (Windows Forms)
    public partial class SMS : Form  
    {
        // Definir variables para la interfaz, nueva instancia de clase chat y puerto local
        private Label? MensajeL;
        private Label? PuertoDestinoL;
        private Label? PuertoLocalL;
        private Label? MensajesEntrantesL;
        private TextBox? EscribirMensajeT;
        private TextBox? PuertoDestinoT;
        private TextBox? PuertoLocalT;
        private TextBox? MensajesEntrantesT;
        private Button? EnviarB;
        private int PuertoLocal;
        private Chat? nuevoChat;

//  Base de clase principal e inicializador de interfaz
    public SMS()
        {
            InitializeComponent();
            IniciarInterfaz();
        }
    
//      Funcion que inicializa la interfaz
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

            //Cajas de texto de la interfaz

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
                Location = new Point(290,55),
                ReadOnly = true,
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

            //Boton en interfaz para enviar mensaje

            EnviarB = new Button
            {
                Text = "Enviar",
                Location = new Point(160,100)
            };
            EnviarB.Click += EnviarClick;
            this.Controls.Add(EnviarB);
        
        //iniciar chat

//      Conseguir numero de puerto insertado en terminal y asignarlo a puerto local
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length > 2 & int.TryParse(args[2], out int puertoParsed))
        {
            PuertoLocal = puertoParsed;
        }
        PuertoLocalT.Text = PuertoLocal.ToString();
        nuevoChat = new Chat(PuertoLocal);
        nuevoChat.OnMsjRecibido += Chat_OnMsjRecibido;
        }

//  Accion al presionar boton de enviar
    void EnviarClick(object sender, EventArgs e)
        {
//      Obtiene mensaje escrito 
        string mensaje = EscribirMensajeT.Text;
        if (int.TryParse(PuertoDestinoT.Text, out int puerto))
        {
//          Envia mensaje a puerto destino
            nuevoChat.EnviandoMensaje($"De puerto {PuertoLocal}: {mensaje}", puerto);
            MensajesEntrantesT.AppendText("Tú: " + mensaje + Environment.NewLine);
            EscribirMensajeT.Clear();
        }
        else
        {
            MessageBox.Show("Ingrese puerto valido");
        }
        }

//  Permite que instancia que ejecute esta funcion imprima el mensaje enviado a este, en caja de texto de mensajes entrantes
    private void Chat_OnMsjRecibido(string mensaje)
    {
        Invoke(new Action(() =>
        {
            MensajesEntrantesT.AppendText(mensaje + Environment.NewLine);
        }
        ));
    }
    }
        
// Clase que maneja conexion de puertos y emision de mensajes
public class Chat
    {
        private Socket socketEnvia;
        private Socket socketRecibe;
        private int puertoRecibe;

//      Base de clase Chat - Define socket receptor y emisor
        public Chat(int puertoRecibe)
        {
            socketEnvia = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketRecibe = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.puertoRecibe = puertoRecibe;
            RecibiendoMensaje();
        }
//      Establece conexion de socket emisor - convierte mensaje en bytes y lo envia a puerto receptor
        public void EnviandoMensaje(string mensaje, int puerto)
        {
            try
            {
                socketEnvia = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketEnvia.Connect(new IPEndPoint(IPAddress.Loopback, puerto));
                byte[] data = Encoding.UTF8.GetBytes(mensaje);
                socketEnvia.Send(data);
                socketEnvia.Shutdown(SocketShutdown.Both);
                socketEnvia.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

//      Siempre busca mensajes y en caso de recibir uno, lo decodifica e imprime en la caja de texto de mensajes recibidos
        public void RecibiendoMensaje()
        {
            Task.Run(() =>   //Permite que mensajes de varios clientes a la vez
            {
                socketRecibe.Bind(new IPEndPoint(IPAddress.Any, puertoRecibe));
                socketRecibe.Listen(10);

                while(true)
                {
                    try
                    {
                        Socket handler = socketRecibe.Accept();
                        byte[] buffer = new byte[1024];
                        int received = handler.Receive(buffer);
                        string msjRecibido = Encoding.UTF8.GetString(buffer, 0, received);
                        OnMsjRecibido?.Invoke(msjRecibido);
                        
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            );
        }
        public event Action<string> OnMsjRecibido;
    }


    