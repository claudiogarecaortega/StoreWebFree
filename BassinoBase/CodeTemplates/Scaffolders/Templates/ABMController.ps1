[T4Scaffolding.Scaffolder(Description = "Enter a description of ABMController here")][CmdletBinding()]
param(     
	[parameter(Mandatory = $true)][string]$Model,
	[parameter(Mandatory = $true)][string]$CarpetaDominio,
    [string]$Project,
	[string]$CodeLanguage = "cs",
	[switch]$Force = $false,
	[string]$ForceMode,
	[switch]$NoChildItems = $false,
	[string[]]$TemplateFolders
)

# Interpret the "Force" and "ForceMode" options
$overwriteController = $Force -and ((!$ForceMode) -or ($ForceMode -eq "ControllerOnly"))
$overwriteFilesExceptController = $Force -and ((!$ForceMode) -or ($ForceMode -eq "PreserveController"))


# Creo el Controller
$ControllerName = $Model + "Controller"
$namespaceController = "MHOWeb.Interfaz.Controllers"
$viewModel = $Model + "ViewModel"

$outputPath = Join-Path Controllers $ControllerName

$templateName = "ABMControllerTemplate"
Add-ProjectItemViaTemplate $outputPath -Template $templateName -Model @{
	ControllerName = $ControllerName;
	ModelName = $Model;
	ViewModelName = $viewModel;
	ControllerNamespace = $namespaceController; 
} -SuccessMessage "Added controller {0}" -TemplateFolders $TemplateFolders -CodeLanguage $CodeLanguage -Project $Project -Force:$overwriteController


# Creo la interfaz del ViewModelMapper
$viewModelMapperInterfaceName = "I" + $Model + "ViewModelMapper"
$namespaceViewModelMapperInterface = "MHOWeb.Interfaz.Models.ViewModelsMappers.Interfaces"

$outputPath = Join-Path "Models/ViewModelsMappers/Interfaces" $viewModelMapperInterfaceName

$templateName = "ViewModelMapperInterfaceTemplate"
Add-ProjectItemViaTemplate $outputPath -Template $templateName -Model @{
	Name = $viewModelMapperInterfaceName;
	Namespace = $namespaceViewModelMapperInterface;
	ModelName = $Model;
	ViewModelName = $viewModel;
} -SuccessMessage "Added ViewModelInterface {0}" -TemplateFolders $TemplateFolders -CodeLanguage $CodeLanguage -Project $Project -Force:$overwriteController


# Creo el ViewModelMapper
$viewModelMapperName = $Model + "ViewModelMapper"
$namespaceModelMapper = "MHOWeb.Interfaz.Models.ViewModelsMappers." + $CarpetaDominio

$outputPath = Join-Path "Models/ViewModelsMappers" $CarpetaDominio
$outputPath = Join-Path $outputPath $viewModelMapper

$templateName = "ViewModelMapperTemplate"
Add-ProjectItemViaTemplate $outputPath -Template $templateName -Model @{
	Name = $viewModelMapperName;
	Namespace = $namespaceModelMapper;
	ModelName = $Model;
	ViewModelName = $viewModel;
	InterfaceNamespace = $namespaceViewModelMapperInterface;
} -SuccessMessage "Added ViewModelInterface {0}" -TemplateFolders $TemplateFolders -CodeLanguage $CodeLanguage -Project $Project -Force:$overwriteController


# Creo Interfaz del DAO
$persistenciaProject = "MHOWeb.Persistencia"
$namespaceDAOInterfaz = "MHOWeb.Persistencia.DAOs.Interfaces"
$IDAOName = "I" + $Model + "DAO"

$outputPath = Join-Path "DAOs/Interfaces" $IDAOName

$templateName = "IDAOTemplate"
Add-ProjectItemViaTemplate $outputPath -Template $templateName -Model @{
	Name = $IDAOName;
	ModelName = $Model;
	Namespace = $namespaceDAOInterfaz;
} -SuccessMessage "Added Interfaz DAO {0}" -TemplateFolders $TemplateFolders -CodeLanguage $CodeLanguage -Project $persistenciaProject -Force:$overwriteController


# Creo el DAO
$namespaceDAO = "MHOWeb.Persistencia.DAOs"
$DAOName = $Model + "DAO"

$outputPath = Join-Path "DAOs" $DAOName

$templateName = "DAOTemplate"
Add-ProjectItemViaTemplate $outputPath -Template $templateName -Model @{
	Name = $DAOName;
	ModelName = $Model;
	Namespace = $namespaceDAO;
} -SuccessMessage "Added DAO {0}" -TemplateFolders $TemplateFolders -CodeLanguage $CodeLanguage -Project $persistenciaProject -Force:$overwriteController




if (!$NoChildItems) {
	$controllerNameWithoutSuffix = [System.Text.RegularExpressions.Regex]::Replace($ControllerName, "Controller$", "", [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)

	Scaffold View -Template Create -ViewName Create -ModelType $viewModel -Controller $controllerNameWithoutSuffix
	Scaffold View -Template Delete -ViewName Delete -ModelType $viewModel -Controller $controllerNameWithoutSuffix
	Scaffold View -Template Details -ViewName Details -ModelType $viewModel -Controller $controllerNameWithoutSuffix
	Scaffold View -Template Edit -ViewName Edit -ModelType $viewModel -Controller $controllerNameWithoutSuffix
	Scaffold View -Template Index -ViewName Index -ModelType $viewModel -Controller $controllerNameWithoutSuffix
}