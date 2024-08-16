using Michsky.UI.Heat;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{

	[SerializeField] private Dropdown resoloutionDropdown;
	private Resolution[] resolutions;
	private int originalResoloutionIndex = 0;

	private void Awake()
	{
		resolutions = Screen.resolutions;
		resoloutionDropdown.items = new List<Dropdown.Item>();

		originalResoloutionIndex = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			resoloutionDropdown.items.Add(new Dropdown.Item());
			resoloutionDropdown.items[i].itemName = option;

			if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
			{
				originalResoloutionIndex = i;
			}
		}

		resoloutionDropdown.SetDropdownIndex(originalResoloutionIndex);
	}

	public void SetResolution(int res) =>
		Screen.SetResolution(resolutions[res].width, resolutions[res].height, Screen.fullScreen);

}
