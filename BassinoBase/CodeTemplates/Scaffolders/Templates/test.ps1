[T4Scaffolding.Scaffolder(Description = "Enter a description of ABMController here")][CmdletBinding()]
param(   
	
)
 $a = 0 
$items = Get-ChildItem -Path "C:\Users\garecaor\Documents\Beta\trunk\Dominio\"
foreach ($item in $items)
{

      # if the item is a directory, then process it.
      if ($item.Attributes -eq "Directory")
      {
           # Write-Host  "$("C:\Users\garecaor\Documents\Beta\trunk\Dominio\")$($item.Name) "
			if($item.Name -ne"bin" -or $item.Name -ne "obj" -or $item.Name -ne "Properties" -or $item.Name -ne "AttachmentDomain")
			{
           
			$files = Get-ChildItem -Path  "$("C:\Users\garecaor\Documents\Beta\trunk\Dominio\")$($item.Name) "
			if($item.Name -ne "ApplicationUserDomain")
			{
				foreach ($file in $files)
				{
				# if the item is NOT a directory, then process it.
						if ($file.Attributes -ne "Directory")
						{
						if($a -eq 0){
							
							Write-Host "Generating..................................."
						    $b="$($s) $($item.Name)"
						 	$s="$($s) $($file.Name)"
							$a=$s.Substring(0, $s.IndexOf('.'))
							$meto="C:\Users\garecaor\Documents\Beta\trunk\Interfaz\CodeTemplates\Scaffolders\Templates\CompleteModel.ps1"
							
							Invoke-Expression "Scaffold C:\Users\garecaor\Documents\Beta\trunk\Interfaz\CodeTemplates\Scaffolders\Templates\CompleteModel.ps1  $a "
							#CompleteModel
							Write-Host "Generated"
							$s="";
								 $a++
								}
								 
							
						}
				}
			}
      }
	  }
}