 using UnityEditor;
 
 [InitializeOnLoad]
 public class PreloadSigningAlias
{
	static PreloadSigningAlias ()
	{
		// place the keystore file in root of project and not in the assets folder...
		// you will have ~/Assets, ~/Library, ~/Project, ~/Temp, ~/myKey.keystore ... etc
		PlayerSettings.Android.keystoreName = "user.keystore";
		PlayerSettings.Android.keystorePass = "9346421";
		PlayerSettings.Android.keyaliasName = "user";
		PlayerSettings.Android.keyaliasPass = "9346421";
	}
 }