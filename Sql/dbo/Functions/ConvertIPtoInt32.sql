CREATE FUNCTION [dbo].[ConvertIPtoInt32]
(
	@IP nvarchar(15)
)
RETURNS BIGINT
AS
BEGIN
	RETURN (
		CONVERT(bigint, isnull(PARSENAME(@IP,4), 0)) * POWER(256,3)
	) + (
		CONVERT(bigint, isnull(PARSENAME(@IP,3), 0)) * POWER(256,2)
	) + (
		CONVERT(bigint, isnull(PARSENAME(@IP,2), 0)) * POWER(256,1)
	) + (
		CONVERT(bigint, isnull(PARSENAME(@IP,1), 0))
	)
END