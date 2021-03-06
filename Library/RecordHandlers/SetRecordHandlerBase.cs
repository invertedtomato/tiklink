﻿using InvertedTomato.TikLink.Records;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InvertedTomato.TikLink.RecordHandlers {
    public abstract class SetRecordHandlerBase<T> : FixedSetRecordHandlerBase<T> where T : SetRecordBase, new() {
        internal SetRecordHandlerBase(Link link) : base(link) { }

        /// <summary>
        /// Add a new record to this set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">Record to be written</param>
        /// <param name="readBack">If TRUE, the record will be updated with any changes the router made during acceptance.</param>
        /// <param name="properties"></param>
        /// <remarks>
        /// ReadBack is really handy to get the ID of the record that's just been created, however it comes at the penalty of an additional query to the router, and could fail to retrieve properties if it detects another Add() that occurs at the same moment.
        /// </remarks>
        public virtual void Add(T record, bool readBack = false, string[] properties = null) {
            if (null == record) {
                throw new ArgumentNullException(nameof(record));
            }
            if (null != record.Id) {
                throw new ArgumentException("Id must be NULL.", nameof(record));
            }

            // If using readBack, get a list of existing IDs first
            IEnumerable<string> beforeIds = null;
            if (readBack) {
                beforeIds = Query("Id").Select(a => a.Id);
            }

            // Get attributes
            var attributes = RecordReflection.GetRosProperties(record);

            // If filtering properties, remove attributes not wanted
            if (null != properties) {
                for(var i = 0; i< properties.Length; i++) {
                    properties[i] = RecordReflection.ResolveProperty<T>(properties[i]);
                }

                var remove = attributes.Keys.Where(k => !properties.Contains(k)).ToList();
                foreach (var k in remove) {
                    attributes.Remove(k);
                }
            }

            // Prepare sentence
            var sentence = new Sentence();
            sentence.Attributes = attributes;
            sentence.Command = RecordReflection.GetPath<T>() + "/add";

            // Make call
            var result = Link.Call(sentence).Wait();
            if (result.IsError) {
                result.TryGetTrapAttribute("message", out var message);
                throw new CallException(message);
            }

            // If using readBack...
            if (readBack) {
                // Get list of IDs
                var afterIds = Query("Id").Select(b => b.Id).ToList();

                // Remove the IDs that existed first
                afterIds.RemoveAll(b => beforeIds.Contains(b));

                // If there's only one new ID (there should be)
                if (afterIds.Count == 1) {
                    // Store Id on record
                    record.Id = afterIds.Single();

                    // Build sentence
                    sentence = new Sentence();
                    sentence.Command = RecordReflection.GetPath<T>() + "/print";
                    sentence.Attributes["detail"] = string.Empty;
                    sentence.Queries.Add($"=.id={record.Id}");

                    // Make call
                    result = Link.Call(sentence).Wait();
                    if (result.IsError) {
                        result.TryGetTrapAttribute("message", out var message);
                        throw new CallException(message);
                    }

                    // Convert record sentence to record
                    var a = result.Sentences.Where(b => b.Command == "re");
                    if (a.Count() != 1) {
                        throw new CallException($"Unexpected number of results returned. Expected 1, got {a.Count()}.");
                    }
                    RecordReflection.SetRosProperties(record, a.Single().Attributes);
                }
            }
        }

        /// <summary>
        /// Delete a record from a set.
        /// </summary>
        /// <remarks>
        /// While it's perfectly fine to use these methods directly, it's intended that you use the vanity methods instead (eg. link.Ip.Arp.Get()).
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        public virtual void Delete(T record) {
            if (null == record) {
                throw new ArgumentNullException(nameof(record));
            }
            if (null == record.Id) {
                throw new ArgumentException("ID cannot be null.", nameof(record));
            }

            // Build sentence
            var sentence = new Sentence();
            sentence.Command = RecordReflection.GetPath<T>() + "/remove";
            sentence.Attributes = new Dictionary<string, string>() {
                {".id", record.Id }
            };

            // Make call
            var result = Link.Call(sentence).Wait();
            if (result.IsError) {
                result.TryGetTrapAttribute("message", out var message);
                throw new CallException(message);
            }
        }
    }
}
