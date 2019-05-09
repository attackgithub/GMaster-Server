/* check if plans table is populated */
IF (SELECT COUNT(*) FROM Plans) < 1 BEGIN
	/* create list of plans */
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (1, 'Beginner', 9.95, 1, 1, 0)
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (2, 'Daily', 14.95, 1, 1, 0)
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (3, 'Pro', 24.95, 1, 1, 0)
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (4, 'Team', 19.95, 2, 10000, 0)
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (5, 'Beginner', 99.95, 1, 1, 1)
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (6, 'Daily', 159.95, 1, 1, 1)
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (7, 'Pro', 269.95, 1, 1, 1)
	INSERT INTO Plans (planId, [name], price, minUsers, maxUsers, schedule) VALUES (8, 'Team', 189.95, 2, 10000, 1)
END 
