using EduHome.Models;

namespace EduHome.ViewModels;

public class HomeVM
{
    public List<HeaderSlider> HomeSlider { get; set; }
    public List<HomeNoticePage> HomeNoticePage { get; set; }
    public List<Event> Event { get; set; }
    public List<Testimonial> HomeFooterSlider { get; set; }
}