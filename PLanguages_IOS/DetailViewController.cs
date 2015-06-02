using System;

using UIKit;
using Foundation;

namespace PLanguages_IOS
{
	public partial class DetailViewController : UIViewController
	{

		public Language DetailItem { get; set; }

		public DetailViewController (IntPtr handle) : base (handle)
		{
		}

		public void SetDetailItem (Language newDetailItem)
		{
			if (DetailItem != newDetailItem) {
				DetailItem = newDetailItem;
				
				// Update the view
				ConfigureView ();
			}
		}

		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (IsViewLoaded && DetailItem != null) {


				Title = DetailItem.Name + "  " + DetailItem.Year; 
				var urlString = DetailItem.Url;
				detailDescriptionLabel.Text = urlString;

				var url = new NSUrl (urlString);
				var request = new NSUrlRequest (url);
				detailWebView.LoadRequest (request);
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


