using System;
using System.Collections;
using System.Collections.Generic;

namespace ClientCommanding.Models {
	public class InMemoryDatabase : IEnumerable<object> {
		private static List<object> instances = new List<object>();

		static InMemoryDatabase() {
			instances.AddRange(new object[]
			                   	{
			                   		new User { Id = 1, Name = "Jeremy" },
									new User { Id = 2, Name = "Foo" },
									new User { Id = 3, Name = "Bar"}
						});
		}

		public void Add(object instance) {
			instances.Add(instance);
		}

		public void Remove(object instance) {
			instances.Remove(instance);
		}

		public IEnumerator<object> GetEnumerator() {
			return instances.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}