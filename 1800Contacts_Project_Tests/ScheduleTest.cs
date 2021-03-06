﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _1800Contacts_Project.Models;

namespace _1800Contacts_Project_Tests
{
    [TestClass]
    public class ScheduleTest
    {
        public Schedule schedule;
        public Course head;
        public Course tail;
        public string headName = "head";
        public string tailName = "tail";

        public string name1 = "name1";
        public string name2 = "name2";
        public string name3 = "name3";
        public string name4 = "name4";
        public string name5 = "name5";

        [TestInitialize]
        public void SetUp()
        {
            schedule = new Schedule();
            head = new Course(headName);
            tail = new Course(tailName, headName);
        }

        [TestMethod]
        public void TestHead()
        {
            Assert.IsNull(schedule.Head);
            Assert.AreEqual(0, schedule.NumCourses);
            schedule.AddCourse(head);
            Assert.AreEqual(head, schedule.Head);
            Assert.AreEqual(1, schedule.NumCourses);
        }

        [TestMethod]
        public void TestTail()
        {
            tail = new Course(tailName);
            Assert.IsNull(schedule.Tail);
            Assert.AreEqual(0, schedule.NumCourses);
            schedule.AddCourse(tail);
            Assert.AreEqual(tail, schedule.Tail);
            Assert.AreEqual(1, schedule.NumCourses);
        }

        [TestMethod]
        public void TestAddCourse()
        {
            Assert.AreEqual(0, schedule.NumCourses);
            Assert.IsTrue(schedule.AddCourse(head));
            Assert.AreEqual(1, schedule.NumCourses);
            Assert.IsTrue(schedule.AddCourse(tail));
            Assert.AreEqual(2, schedule.NumCourses);
            Assert.AreEqual(head, schedule.Head);
            Assert.AreEqual(tail, schedule.Tail);
            Course course = new Course(name3, tailName);
            Assert.IsTrue(schedule.AddCourse(course));
            Assert.AreEqual(course, schedule.Tail);
        }

        [TestMethod]
        public void TestRemoveCourse()
        {
            schedule.AddCourse(head);
            Assert.AreEqual(1, schedule.NumCourses);
            Assert.IsTrue(schedule.RemoveCourse(head));
            Assert.AreEqual(0, schedule.NumCourses);
        }

        [TestMethod]
        public void TestFindPrerequisite()
        {
            schedule.AddCourse(head);
            schedule.AddCourse(tail);
            Assert.AreEqual(head, schedule.FindPrerequisite(tail));
        }

        [TestMethod]
        public void TestOneCourse()
        {
            Assert.AreEqual(name1, schedule.GetSchedule(new string[] {name1}));
        }

        [TestMethod]
        public void TestCourseWithPrerequisite()
        {
            string course1 = name1 + ": " + name2;
            string course2 = name2;
            string expected = name2 + ", " + name1;
            Assert.AreEqual(expected, schedule.GetSchedule(new string[] {course1, course2}));
        }

        [TestMethod]
        public void TestMultipleCoursesWithPrerequisites()
        {
            string course1 = getCourseString(name1, name2);
            string course2 = getCourseString(name2, name3);
            string course3 = name3;
            string course4 = getCourseString(name4, name2);

            string expected = name3 + ", " + name2 + ", " + name4 + ", " + name1;
            Assert.AreEqual(expected, schedule.GetSchedule(new string[] { course1, course2, course3, course4 }));
        }

        [TestMethod]
        public void TestCircularDependency()
        {
            string course1 = getCourseString(name1, name2);
            string course2 = getCourseString(name2, name1);

            try
            {
                schedule.GetSchedule(new string[] { course1, course2 });
                Assert.Fail("Schedule with circular dependency was allowed.");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Course causes circular dependency!", e.Message);
            }
        }

        [TestMethod]
        public void TestMultipleCoursesWithCircularDependency()
        {
            string course1 = getCourseString(name1, name4);
            string course2 = getCourseString(name2, name1);
            string course3 = getCourseString(name3, name4);
            string course4 = getCourseString(name4, name2);

            try
            {
                schedule.GetSchedule(new string[] { course1, course2, course3, course4 });
                Assert.Fail("Schedule with circular dependency was allowed.");
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual("Course causes circular dependency!", e.Message);
            }
        }

        private string getCourseString(string name1, string name2)
        {
            return name1 + ": " + name2;
        }
    }
}
