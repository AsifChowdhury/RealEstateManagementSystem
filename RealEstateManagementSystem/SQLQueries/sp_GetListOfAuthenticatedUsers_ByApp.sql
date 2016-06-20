USE [CentralDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetListOfAuthenticatedUsers_ByApp]    Script Date: 14/Jun/2016 2:01:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[sp_GetListOfAuthenticatedUsers_ByApp]
	@applicationId int, @returnAuthorizedOnly bit = 0
AS
IF @returnAuthorizedOnly = 0
	SELECT	dbo.Employee.EmpId, 
			dbo.EmployeeInfo.CompanyId, 
			dbo.EmployeeInfo.Name, 
			dbo.fnGetCurrentPosition(dbo.EmployeeInfo.CompanyId) AS Designation, 
			ISNULL((SELECT isAllowed FROM dbo.ApplicationPermission WHERE (EmpId = dbo.Employee.EmpId) AND (ApplicationId = @applicationId)), 0) 
				AS IsAllowed
	FROM	dbo.Employee INNER JOIN
			dbo.EmployeeInfo ON dbo.Employee.PFNumber = dbo.EmployeeInfo.CompanyId
	WHERE   (dbo.EmployeeInfo.IsActive = 1)
	ORDER BY dbo.Employee.EmpId
ELSE
		SELECT	dbo.Employee.EmpId, 
			dbo.EmployeeInfo.CompanyId, 
			dbo.EmployeeInfo.Name, 
			dbo.fnGetCurrentPosition(dbo.EmployeeInfo.CompanyId) AS Designation, 
			ISNULL((SELECT isAllowed FROM dbo.ApplicationPermission WHERE (EmpId = dbo.Employee.EmpId) AND (ApplicationId = @applicationId)), 0) 
				AS IsAllowed
	FROM	dbo.Employee INNER JOIN
			dbo.EmployeeInfo ON dbo.Employee.PFNumber = dbo.EmployeeInfo.CompanyId
	WHERE   (dbo.EmployeeInfo.IsActive = 1)
			AND ISNULL((SELECT isAllowed FROM dbo.ApplicationPermission WHERE (EmpId = dbo.Employee.EmpId) AND (ApplicationId = @applicationId)), 0) = 1
	ORDER BY dbo.Employee.EmpId

--SELECT EmpId, EmployeeName, Designation, PFNumber, 
--	   ISNULL((SELECT isAllowed FROM ApplicationPermission WHERE EmpId=Employee.EmpId AND ApplicationId=@applicationId) ,0) AS IsAllowed 
--	   FROM Employee WHERE Active=1 ORDER BY EmpId