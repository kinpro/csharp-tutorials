-- 1. Using function

select
	id,
	contract_code,
	dbo.TutGetOlb(id) olb
	--dbo.GetOLB(id, getdate())
from
	dbo.Contracts


-- 2. Using sets
select
	c.id,
	c.contract_code,
	sum(i.principal - i.paid_principal) olb
from
	dbo.Contracts c
left join
	dbo.InstallmentSnapshot(getdate()) i on i.contract_id = c.id
group by
	c.id,
	c.contract_code