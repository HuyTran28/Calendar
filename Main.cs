using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Calendar
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer, true);
            
            dateTimePicker.Value = DateTime.Now;
            update();
            Timer timer = new Timer();
            timer.Interval = (1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            updateTime();
        }

        private Dictionary<String, String> dayOfWeekDict = new Dictionary<String, String> {
            {"Monday", "Thứ hai" },
            {"Tuesday", "Thứ ba" },
            {"Wednesday", "Thứ tư" },
            {"Thursday", "Thứ năm" },
            {"Friday", "Thứ sáu" },
            {"Saturday", "Thứ bảy" },
            {"Sunday", "Chủ nhật" }
        };

        private string[] celestialStem = {
            "Giáp", "Ất", "Bính", "Đinh", "Mậu",
            "Kỷ", "Canh", "Tân", "Nhâm", "Quý"
        };

        private string[] terrestrialBranch = {
            "Tý", "Sửu", "Dần", "Mão", "Thìn",
            "Tỵ", "Ngọ", "Mùi", "Thân", "Dậu",
            "Tuất", "Hợi"
        };

        private string[] quotes = {
            "Dù người ta có nói với bạn điều gì đi nữa, hãy tin rằng cuộc sống là điều kỳ diệu và đẹp đẽ|Pautopxki",
            "Một nửa sức khỏe của con người là trong tâm lý|S.Aleksievist",
            "Có niềm tin mà không hành động, niềm tin đó có thành khẩn hay không?|A.Maurois",
            "Ngu dốt không đáng thẹn bằng thiếu ý chí học hỏi|B.Franklin",
            "Nếu bạn muốn đi qua cuộc đời không phiền toái thì chẳng nên bỏ đá vào túi mà đeo|V.Shemtchisnikov",
            "Lòng tin không phải là khởi đầu mà là kết quả của mọi nhận thức|W.Goethe",
            "Thiên đường ở chính trong ta. Địa ngục cũng do lòng ta mà có.|Chúa Jésus",
            "Tôi thường hối tiếc vì mình đã mở mồm chứ không bao giờ…vì mình đã im lặng|Philippe de Commynes",
            "Khiêm tốn bao nhiêu cũng chưa đủ, tự kiêu một chút cũng là nhiều|Karl Marx",
            "Đừng bao giờ khiêm tốn với kẻ kiêu căng, cũng đừng bao giờ kiêu căng với người khiêm tốn|Jeffecson",
            "Không có gì hèn cho bằng khi ta nghĩ bạo mà không dám làm|Jean Ronstard",
            "Tất cả mọi người đều ao ước có được nhiều hiểu biết, điều kiện đầu tiên là phải biết nhìn dời với cặp mắt của đứa trẻ thơ, cái gì cũng mới lạ và làm cho ta ngạc nhiên cả|Aristot",
            "Sáng nào cũng vậy, khi tôi thức giấc tôi đều tin chắc sắp có sự việc thích thú xảy ra và không bao giờ tôi bị thất vọng|Elsa Maxwell",
            "Biết bao kẻ đọc sách và học hỏi, không phải để tìm ra chân lý mà là để gia tăng những gì mình đã biết|Julien Green",
            "Những kẻ trí tuệ tầm thường hay lên án những gì vượt quá tầm hiểu biết của họ|La Rochefoucould",
            "Nếu ai nói xấu bạn mà nói đúng thì hãy sửa mình đi. Nếu họ nói bậy thì bạn hãy cười thôi|Epictete",
            "Ai không biết nghe, tất không biết nói chuyện|Giarardin",
            "Phải biết mở cửa lòng mình trước mới hy vọng mở được lòng người khác|Pasquier Quesnel",
            "Cái nhìn vui vẻ biến một bữa ăn thành một bữa tiệc|Herbert",
            "Im lặng và khiêm tốn là đặc tính rất quý trong cuộc đàm thoại|Monteigne",
            "Bạn nghi ngờ ai tùy bạn, nhưng đừng nghi ngờ bản thân mình|Plutarch",
            "Điều oái oăm là, nếu bạn không muốn liều mất cái gì thì bạn còn mất nhiều hơn|Erica Jong",
            "Không một người nào đã từng cười hết mình và cười xả láng lại đồng thời là người xấu xa.|Thomas Carlyle",
            "Đừng tự hạ giá bạn. Tất cả những gì bạn có đã làm nên nhân cách của bạn|Janis Joplin",
            "Không có điều gì trên đời khiến chúng ta phải sợ. Chỉ có những điều chúng ta cần phải hiểu|Marie Curie",
            "Cố chấp và bảo thủ là bằng chứng chắc chắn nhất của sự ngu si.|J.b.Bactông",
            "Bạn có thể thất vọng nếu bạn tin quá nhiều nhưng bạn có thể sống trong sự giày vò nếu bạn tin không đủ|Alexander Smith",
            "Người nào đó không dám làm gì hết, đừng hy vọng gì cả|Schillet",
            "Biết xấu hổ trước mọi người là một cảm xúc tốt. Nhưng tốt hơn hết là biết xấu hổ trước chính bản thân mình|Lep-tônxtôi",
            "Người nghiêm túc không bao giờ lấy chiếc lá nhỏ để che đậy sự thật trần trụi|G.L.Boan",
            "Chỉ có những thằng ngốc và người chết là chẳng bao giờ thay đổi ý kiến|S.Saplin",
            "Hay nóng giận nản lòng là triệu chứng của một tâm hồn yếu đuối|Điđơrô",
            "Không tự tin là nguyên nhân gây ra tất cả thất bại|Bôuvi",
            "Cái bệnh nặng nhất của đời sống, ấy là sự buồn nản|A.Devigni",
        };

        private Dictionary<String, String> fest = new Dictionary<String, String> {
            {"1/1", "Tết Dương lịch"},
            {"9/1", "Ngày Học sinh – Sinh viên Việt Nam"},
            {"3/2", "Ngày thành lập Đảng Cộng sản Việt Nam"},
            {"4/2", "Ngày ung thư thế giới"},
            {"14/2", "Lễ tình nhân (Valentine)"},
            {"27/2", "Ngày thầy thuốc Việt Nam"},
            {"8/3", "Ngày Quốc tế Phụ nữ"},
            {"20/3", "Ngày Quốc tế Hạnh phúc"},
            {"22/3", "Ngày Nước sạch Thế giới"},
            {"24/3", "Ngày Thế giới phòng chống lao"},
            {"26/3", "Ngày thành lập Đoàn TNCS Hồ Chí Minh"},
            {"27/3", "Ngày Thể thao Việt Nam"},
            {"28/3", "Ngày thành lập lực lượng Dân quân tự vệ"},
            {"1/4", "Ngày Cá tháng Tư"},
            {"2/4", "Ngày Thế giới Nhận thức Tự kỷ"},
            {"22/4", "Ngày Trái đất"},
            {"23/4", "Ngày Sách Việt Nam"},
            {"30/4", "Ngày giải phóng miền Nam"},
            {"1/5", "Ngày Quốc tế Lao động"},
            {"7/5", "Ngày chiến thắng Điện Biên Phủ"},
            {"13/5", "Ngày của mẹ"},
            {"15/5", "Ngày thành lập Đội Thiếu niên Tiền phong Hồ Chí Minh"},
            {"19/5", "Ngày sinh chủ tịch Hồ Chí Minh"},
            {"1/6", "Ngày Quốc tế thiếu nhi"},
            {"5/6", "Ngày Bác Hồ ra đi tìm đường cứu nước  và Ngày môi trường thế giới"},
            {"17/6", "Ngày của cha"},
            {"21/6", "Ngày báo chí Việt Nam"},
            {"28/6", "Ngày gia đình Việt Nam"},
            {"11/7", "Ngày dân số thế giới"},
            {"27/7", "Ngày Thương binh liệt sĩ"},
            {"28/7", "Ngày thành lập công đoàn Việt Nam"},
            {"19/8", "Ngày tổng khởi nghĩa, Ngày Cách mạng tháng Tám thành công"},
            {"2/9", "Ngày Quốc Khánh"},
            {"7/9", "Ngày thành lập Đài Truyền hình Việt Nam"},
            {"10/9", "Ngày thành lập Mặt trận Tổ quốc Việt Nam"},
            {"1/10", "Ngày quốc tế người cao tuổi"},
            {"10/10", "Ngày giải phóng thủ đô"},
            {"13/10", "Ngày doanh nhân Việt Nam"},
            {"14/10", "Ngày thành lập Hội Nông dân Việt Nam"},
            {"20/10", "Ngày Phụ nữ Việt Nam và cũng là Ngày thành lập Hội Phụ nữ Việt Nam"},
            {"31/10", "Ngày Hallowen"},
            {"9/11", "Ngày pháp luật Việt Nam"},
            {"20/11", "Ngày Nhà giáo Việt Nam"},
            {"23/11", "Ngày thành lập Hội chữ thập đỏ Việt Nam"},
            {"1/12", "Ngày thế giới phòng chống AIDS"},
            {"19/12", "Ngày toàn quốc kháng chiến"},
            {"24/12", "Ngày lễ Giáng sinh"},
            {"22/12", "Ngày thành lập quân đội nhân dân Việt Nam"}
        };

        private Dictionary<String, String> lunarFest = new Dictionary<String, String> {
            {"1/1", "Tết Nguyên Đán"},
            {"15/1", "Tết Nguyên Tiêu (Lễ Thượng Nguyên)"},
            {"3/3", "Tết Hàn Thực"},
            {"10/3", "Giỗ Tổ Hùng Vương"},
            {"15/4", "Lễ Phật Đản"},
            {"5/5", "Tết Đoan Ngọ"},
            {"15/7", "Lễ Vu Lan"},
            {"15/8", "Tết Trung Thu"},
            {"9/9", "Tết Trùng Cửu"},
            {"10/10", "Tết Thường Tân"},
            {"15/10", "Tết Hạ Nguyên"},
            {"23/12", "Tiễn Táo Quân về trời"},
        };

        private bool isUpdating = true;

        private string Festival(DateTime date)
        {
            string res = "";
            ChineseLunisolarCalendar lunar = new ChineseLunisolarCalendar();

            if (fest.ContainsKey(date.ToString("d/M")))
            {
                res = fest[date.ToString("d/M")];
            }

            int day = lunar.GetDayOfMonth(date);
            int leapMonth = lunar.GetLeapMonth(lunar.GetYear(date), lunar.GetEra(date));
            int curMonth = lunar.GetMonth(date);
            if(leapMonth != curMonth)
            {
                if((leapMonth > 0) && (curMonth > leapMonth))
                {
                    curMonth -= 1;
                }

                string key = day.ToString() + "/" + curMonth.ToString();
                if(lunarFest.ContainsKey(key))
                {
                    if (res != "")
                        res += "\n";
                    res += lunarFest[key];
                }
            }

            return res;
        }

        private void updateTime()
        {
            time_label.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void update()
        {
            if (!isUpdating)
                return;
            var now = dateTimePicker.Value;
            if (now <= dateTimePicker.MinDate)
            {
                buttonLeft.Enabled = false;
                buttonLeft.Visible = false;
            }
            else
            {
                buttonLeft.Enabled = true;
                buttonLeft.Visible = true;
            }

            if (now >= dateTimePicker.MaxDate)
            {
                buttonRight.Enabled = false;
                buttonRight.Visible = false;
            }
            else
            {
                buttonRight.Enabled = true;
                buttonRight.Visible = true;
            }
            year_label.Text = now.Year.ToString();
            month_label.Text = "Tháng " + now.Month.ToString();
            sunDayOfYear.Text = now.Day.ToString();
            dayOfWeek.Text = dayOfWeekDict[now.DayOfWeek.ToString()];

            ChineseLunisolarCalendar lunar = new ChineseLunisolarCalendar();
            lunarDayOfYear.Text = lunar.GetDayOfMonth(now).ToString();
            int leapMonth = lunar.GetLeapMonth(lunar.GetYear(now), lunar.GetEra(now));
            int curMonth = lunar.GetMonth(now);
            if(leapMonth > 0 && (curMonth >= leapMonth))
            {
                lunarMonth_label.Text =
                    "Tháng " +
                    (curMonth - 1).ToString() +
                    ((curMonth == leapMonth) ? " (N)" : "");
                lunarMonth.Text = "Tháng " +
                    celestialStem[(lunar.GetYear(now) * 12 + curMonth + 2) % 10] + " " +
                    terrestrialBranch[curMonth%12] +
                    ((curMonth == leapMonth) ? " (N)" : "");
            }
            else
            {
                lunarMonth_label.Text = "Tháng " + (curMonth).ToString();
                lunarMonth.Text = "Tháng " +
                    celestialStem[(lunar.GetYear(now) * 12 + curMonth + 3) % 10] + " " +
                    terrestrialBranch[(curMonth + 1)%12] +
                    ((lunar.GetDaysInMonth(lunar.GetYear(now), lunar.GetMonth(now), lunar.GetEra(now)) == 29)?" (T)":" (Đ)");
            }
            int julianDate = (int)(now.Date.ToOADate() + 2415019);
            lunarDay.Text =
                "Ngày " + 
                celestialStem[(julianDate+9)%10] + " " +
                terrestrialBranch[(julianDate+1)%12];
            int sexagenaryYear = lunar.GetSexagenaryYear(now);
            lunarYear.Text =
                "Năm "
                + celestialStem[lunar.GetCelestialStem(sexagenaryYear) - 1] + " "
                + terrestrialBranch[lunar.GetTerrestrialBranch(sexagenaryYear) - 1]
                + (lunar.IsLeapYear(lunar.GetYear(now))?" (N)":"");

            var quote = quotes[now.DayOfYear % quotes.Length].Split('|');


            string lunDay = "";
            string solDay = now.ToString("d/M");
            int day = lunar.GetDayOfMonth(now);
            if (leapMonth != curMonth)
            {
                if ((leapMonth > 0) && (curMonth > leapMonth))
                {
                    curMonth -= 1;
                }

                lunDay = day.ToString() + "/" + curMonth.ToString();
            }

            if (lunDay == "1/1")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("Tet1");
            } else if (lunDay == "2/1")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("Tet2");
            } else if (lunDay == "3/1")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("Tet3");
            } else if (lunDay == "15/8")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("Mid-Au");
            } else if (solDay == "14/2")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("_14_2");
            } else if (solDay == "25/12")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("_25_12");
            } else if (solDay == "30/4")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("_30_4");
            } else if (solDay == "31/10")
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("_31_10");
            } else
            {
                this.BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("_" + now.Day.ToString());
            }
            
            var festivalDay = Festival(now);
            if(festivalDay != "")
            {
                quote_label.Text = festivalDay;
                author_label.Text = "";
                quote_label.ForeColor = Color.Red;
                quote_label.Font = new Font(quote_label.Font, FontStyle.Bold);
            } else
            {
                quote_label.Text = "\"" + quote[0] + "\"";
                author_label.Text = quote[1];
                quote_label.ForeColor = Color.Sienna;
                quote_label.Font = new Font(quote_label.Font, FontStyle.Italic);
            }           
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            if (!(e is MouseEventArgs))
                return;
            dateTimePicker.Value = dateTimePicker.Value.AddDays(-1);
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            if (!(e is MouseEventArgs))
                return;
            dateTimePicker.Value = dateTimePicker.Value.AddDays(1);
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            update();
        }

        private void toggleChooseDate_Click(object sender, EventArgs e)
        {
            if (!(e is MouseEventArgs))
                return;
            if (dateTimePicker.Visible)
            {
                dateTimePicker.Visible = false;
                month_label.Top += -38;
                year_label.Top += -38;
                sunDayOfYear.Top += -15;
                dayOfWeek.Top += -15;
            } else
            {
                dateTimePicker.Visible = true;
                month_label.Top += 38;
                year_label.Top += 38;
                sunDayOfYear.Top += 15;
                dayOfWeek.Top += 15;
            }
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Left) && (dateTimePicker.Value > dateTimePicker.MinDate))
            {
                dateTimePicker.Value = dateTimePicker.Value.AddDays(-1);
            }
            else if ((e.KeyData == Keys.Right) && (dateTimePicker.Value < dateTimePicker.MaxDate))
            {
                dateTimePicker.Value = dateTimePicker.Value.AddDays(1);
            }
        }
    }
}
