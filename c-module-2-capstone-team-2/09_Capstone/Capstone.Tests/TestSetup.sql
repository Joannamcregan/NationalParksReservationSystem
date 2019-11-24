DELETE FROM reservation
DELETE FROM site
DELETE FROM campground
DELETE FROM park

INSERT INTO park (name, location, establish_date, area, visitors, description)
VALUES ('TestPark', 'here', GETDATE(), 5000, 1000, 'cool place')

DECLARE @parkId int
SELECT @parkId = @@IDENTITY

INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee)
VALUES (@parkId, 'campLAND', 1, 12, 10.00)

DECLARE @campId int
SELECT @campId = @@IDENTITY

INSERT INTO site (site_number, campground_id) VALUES (1, @campId)

DECLARE @site1ID int
SELECT @site1ID = @@IDENTITY

INSERT INTO site (site_number, campground_id) VALUES (2, @campId)

DECLARE @site2ID int
SELECT @site2ID = @@IDENTITY

INSERT INTO site (site_number, campground_id) VALUES (3, @campId)

DECLARE @site3ID int
SELECT @site3ID = @@IDENTITY

INSERT INTO reservation (site_id, name, from_date, to_date)
VALUES (@site1ID, 'josh family', '2019/11/02', '2019/11/03')

INSERT INTO reservation (site_id, name, from_date, to_date)
VALUES (@site1ID, 'abby family', '2019/12/01', '2019/12/03')
INSERT INTO reservation (site_id, name, from_date, to_date)
VALUES (@site2ID, 'brian family', '2019/12/02', '2019/12/03')
INSERT INTO reservation (site_id, name, from_date, to_date)
VALUES (@site3ID, 'bethany family', '2019/12/02', '2019/12/04')

INSERT INTO reservation (site_id, name, from_date, to_date)
VALUES (@site1ID, 'emily family', '2019/10/01', '2019/10/31')




SELECT @campId AS camp_id



