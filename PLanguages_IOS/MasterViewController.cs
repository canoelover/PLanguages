using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using CoreGraphics;
using System.Xml;

namespace PLanguages_IOS
{
	public partial class MasterViewController : UITableViewController
	{
		public DetailViewController DetailViewController { get; set; }

		private DataSource dataSource;

		public MasterViewController (IntPtr handle) : base (handle)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Computer Languages", "Languages");
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				PreferredContentSize = new CGSize (320f, 600f);
				ClearsSelectionOnViewWillAppear = false;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
//			NavigationItem.LeftBarButtonItem = EditButtonItem;

//			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, AddNewItem);
//			addButton.AccessibilityLabel = "addButton";
//			NavigationItem.RightBarButtonItem = addButton;

			DetailViewController = (DetailViewController)((UINavigationController)SplitViewController.ViewControllers [1]).TopViewController;

			TableView.Source = dataSource = new DataSource (this);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail") {

				var indexPath = TableView.IndexPathForSelectedRow;
				var item = dataSource.GetItem (indexPath.Row);
				var controller = (DetailViewController)((UINavigationController)segue.DestinationViewController).TopViewController;
				controller.SetDetailItem (item);
				controller.NavigationItem.LeftBarButtonItem = SplitViewController.DisplayModeButtonItem;
				controller.NavigationItem.LeftItemsSupplementBackButton = true;



			}
		}

	}


	//==                          DataSource                            ==



	/// <summary>
	/// Data source class.  
	/// </summary>
	class DataSource : UITableViewSource
	{
		string CellIdentifier = "Cell";
		List<Language> languages = new List<Language> ();
		readonly MasterViewController controller;


		public DataSource (MasterViewController controller)
		{
			this.controller = controller;

			GetLanguagesFromXML ("languages.xml");
		}


		// Customize the number of sections in the table view.
		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return languages.Count;
		}

		// Customize the appearance of table view cells.
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (CellIdentifier, indexPath);

			cell.TextLabel.Text = languages [indexPath.Row].Name;

			cell.DetailTextLabel.Text = languages [indexPath.Row].Year;			

			return cell;
		}

		public Language GetItem (int id)
		{
			return languages [id];
		}


		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			// Return false if you do not want the specified item to be editable.
			return false;
		}
			
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				controller.DetailViewController.SetDetailItem (languages [indexPath.Row]);
		}
			
		/// <summary>
		/// This method parses the xml file lanuages.xml
		/// </summary>
		/// <param name="path">Path.</param>
		private void GetLanguagesFromXML (string path)
		{
			XmlTextReader reader = new XmlTextReader (path);
			Language currentLanguage = null;
			while (reader.Read ()) {
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "LANGUAGE") {
					currentLanguage = new Language ();
				} else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "LANGUAGE") {
					languages.Add (currentLanguage);
				} else {
					switch (reader.Name) {
					case "NAME":
						currentLanguage.Name = reader.ReadElementContentAsString ();
						break;
					case "YEAR":
						currentLanguage.Year = reader.ReadElementContentAsString ();
						break;
					case "URL":
						currentLanguage.Url = reader.ReadElementContentAsString ();
						break;
					}
				}
			}
		}

	}
}

